using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class MainGenerationVar : MonoBehaviour
{
    [SerializeField]
    private int planetPartsCount;
    private int planetParts = 0;
    [SerializeField]
    private int octave;
    [SerializeField]
    private float power;
    [SerializeField]
    private float scale;
    [SerializeField]
    private float force;
    [SerializeField]
    private float multiplier;
    [SerializeField]
    private Vector3 startCoord;
    private float[][] heights;
    private Vector3[][] vertices;
    private Mesh[] meshes;
    private MeshCollider[] meshColliders;
    private bool allParts;

    private void Awake() 
    {
        vertices = new Vector3 [planetPartsCount][];
        heights = new float [planetPartsCount][];
        meshes = new Mesh [planetPartsCount];
        meshColliders = new MeshCollider [planetPartsCount];
        StartCoroutine (Generate ());
    }

    public void AddPlanetPart (Mesh mesh, MeshCollider mc)
    {
        vertices[planetParts] = new Vector3 [mesh.vertexCount];
        heights[planetParts] = new float [mesh.vertexCount];
        meshes[planetParts] = mesh;
        meshColliders[planetParts] = mc;
        vertices[planetParts] = mesh.vertices;
        planetParts ++;
        if (planetParts == planetPartsCount)
        {
            allParts = true;
        }
    }
    
    IEnumerator Generate ()
    {
        yield return new WaitUntil (() => allParts == true);
        PerlinGenerationJob [] perlinGenerations = new PerlinGenerationJob [planetPartsCount];
        NativeArray<Vector3>[] vert = new NativeArray<Vector3> [planetPartsCount];
        NativeArray<float>[] hei = new NativeArray<float> [planetPartsCount];
        var op = new JobHandle [planetPartsCount];
        for (int i = 0; i < planetPartsCount; i++)
        {
            vert [i] = new NativeArray<Vector3> (vertices[i].Length, Allocator.Persistent);
            hei [i] = new NativeArray<float> (vertices[i].Length, Allocator.Persistent);
            for (int j = 0; j < vertices[i].Length; j++)
            {
                vert[i][j] = vertices[i][j];
                hei[i][j] = heights[i][j];
            }
            perlinGenerations[i] = new PerlinGenerationJob ()
            {
                Vertices = vert[i],
                Heights = hei[i],
                Octave = octave,
                StartCoord = startCoord,
                Scale = scale,
                Force = force,
                Power = power,
                Multiplier = multiplier
            };
            op[i] = perlinGenerations[i].Schedule (vertices[i].Length, 0);
        }
        for (int i = 0; i < planetPartsCount; i++)
        {
            op[i].Complete ();
            for (int j = 0; j < vertices[i].Length; j++)
            {
                vertices[i][j] = vert[i][j];
            }
            meshes[i].vertices = vertices[i];
            hei[i].Dispose ();
            vert[i].Dispose ();
            meshes[i].RecalculateBounds ();
            meshes[i].RecalculateNormals ();
            meshColliders[i].sharedMesh = meshes[i];
        }
    }
}
