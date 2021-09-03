using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


public class TerrainGeneration : MonoBehaviour
{
    public float power;
    public float force;
    public float scale;
    private Transform tr;
    private Mesh mesh;
    private MeshCollider mc;
    private float [] heights;
    private Vector3[] vertices;
    [SerializeField]
    private Vector3 startCoord;
    [SerializeField]
    private int octave;
    [SerializeField]
    private float multiplier = 1;
    [SerializeField]
    private Transform player;

    void Start () 
    {
        PerlinNoise3D.SeedGenerator (42);
        PerlinNoise3D.NoiseGenerator (256);
        mc = this.GetComponent <MeshCollider> ();
        tr = this.GetComponent <Transform> ();
        mesh = this.GetComponent <MeshFilter> ().mesh;
        heights = new float [mesh.vertices.Length];
        vertices = mesh.vertices;
        var vert = new NativeArray<Vector3> (vertices.Length, Allocator.Persistent);
        var hei = new NativeArray<float> (heights.Length, Allocator.Persistent);
        for (int i = 0; i < vertices.Length; i++)
        {
            vert [i] = vertices [i];
            hei [i] = heights [i];
        }
        var perlinJob = new PerlinGenerationJob ()
        {
            Vertices = vert,
            Heights = hei,
            Octave = octave,
            StartCoord = startCoord,
            Scale = scale,
            Force = force,
            Power = power,
            Multiplier = multiplier
        };
        var pj = perlinJob.Schedule (vertices.Length, 0);
        pj.Complete ();
        for (int i = 0; i < heights.Length; i++)
        {
            vertices [i] = perlinJob.Vertices [i];
            heights [i] = perlinJob.Heights [i];
        }
        vert.Dispose ();
        hei.Dispose ();
        mesh.vertices = vertices;
        mesh.RecalculateBounds (); 
        mesh.RecalculateNormals ();
        mc.sharedMesh = mesh;
        if (player != null)
        {
            player.position = vertices [0]*100 + vertices [0].normalized * 100;
        }
    }
        
}
