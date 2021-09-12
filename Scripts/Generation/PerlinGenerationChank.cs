using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class PerlinGenerationChank : MonoBehaviour
{
    [SerializeField]
    int chunkSize;
    [SerializeField]
    int chunkBlockSize;
    [SerializeField]
    float scale;
    [SerializeField]
    MeshFilter cube;
    [SerializeField]
    Material material;
    Mesh cubeMesh;
    void Start()
    {

        cubeMesh = cube.mesh;
        MathU.SeedGenerator (42);
        MathU.NoiseGenerator (256);

        ChunkGenerationJob.ChunkSize = chunkSize;
        ChunkGenerationJob.Scale = scale;
        ChunksGenerationJob.ChunkLoaderTransform = this.transform;
        ChunksGenerationJob.ChunkSize = chunkSize;
        ChunksGenerationJob.CubeMaterial = material;
        ChunksGenerationJob.CubeMesh = cubeMesh;
        ChunkBlockLoad (new Vector3 (0,0,0));

    }
    private void ChunkBlockLoad (Vector3 chunkBlockLocation)
    {
        var o = new NativeArray<Vector3> (chunkBlockSize * chunkBlockSize * chunkBlockSize, Allocator.Persistent);
        var p = new NativeArray <CombineInstance> (chunkBlockSize * chunkBlockSize * chunkBlockSize * chunkSize * chunkSize * chunkSize, Allocator.Persistent);
        for (int i = 0; i < chunkBlockSize; i++)
        {
            for (int j = 0; j < chunkBlockSize; j++)
            {
                for (int k = 0; k < chunkBlockSize; k++)
                {
                    o [i * chunkBlockSize * chunkBlockSize + j * chunkBlockSize + k] = new Vector3 (chunkBlockLocation.x * chunkBlockSize + i, 
                                                                                                    chunkBlockLocation.y * chunkBlockSize + j, 
                                                                                                    chunkBlockLocation.z * chunkBlockSize + k);
                }
            }
        }
        var op = new ChunksGenerationJob
        {
            ChunksLocations = o,
            CombineInstances = p
        };
        var ju = op.Schedule (o.Length, 0);
        ju.Complete ();
        var t = new CombineInstance [chunkBlockSize * chunkBlockSize * chunkBlockSize] [];
        for (int i = 0; i < chunkBlockSize * chunkBlockSize * chunkBlockSize; i++)
        {
            t[i] = new CombineInstance [chunkSize * chunkSize * chunkSize];
            for (int j = 0; j < chunkSize * chunkSize * chunkSize; j++)
            {
                t[i][j] = p [i * chunkSize * chunkSize * chunkSize + j];
            }
            CombineInNew (t[i]);
        }
        p.Dispose ();
        o.Dispose ();
    }
    private void ChunkLoad (Vector3 chunkLocation)
    {
        var num = new NativeArray<float> (chunkSize * chunkSize * chunkSize, Allocator.Persistent);
        var chunkGeneration = new ChunkGenerationJob
        {
            ChunkLocation = chunkLocation,
            CombinedNum = num
        };
        var o = chunkGeneration.Schedule (chunkSize * chunkSize * chunkSize, 0);
        o.Complete ();
        var colMesh = new CombineInstance [chunkSize * chunkSize * chunkSize + 1];
        for (int i = 0; i < chunkSize * chunkSize * chunkSize; i++)
        {
            if (chunkGeneration.CombinedNum [i] == 1)
            {
                colMesh[i].mesh = cubeMesh;
                var ju = new GameObject();
                ju.transform.position = (new Vector3 (((i - (i % chunkSize) - (((i - (i % chunkSize)) / chunkSize) % chunkSize)) / chunkSize / chunkSize), (((i - (i % chunkSize)) / chunkSize) % chunkSize), (i % chunkSize)) + chunkLocation*chunkSize);
                colMesh [i].transform = ju.transform.localToWorldMatrix;
                GameObject.Destroy (ju);
            }
        }
        num.Dispose ();
        var chunk = new GameObject ("Chunk" + chunkLocation.x.ToString () + chunkLocation.y.ToString () + chunkLocation.y.ToString (), typeof (MeshFilter), typeof (MeshRenderer));
        chunk.transform.parent = this.transform;
        chunk.GetComponent <MeshFilter> ().mesh.CombineMeshes (colMesh);
        chunk.GetComponent <MeshFilter> ().mesh.RecalculateBounds ();
        chunk.GetComponent <MeshFilter> ().mesh.RecalculateNormals ();
        chunk.GetComponent <MeshRenderer> ().material = material;
    }
    private GameObject CombineInNew (CombineInstance[] col)
    {
        var chunk = new GameObject ("Chunk", typeof (MeshFilter), typeof (MeshRenderer));
        chunk.GetComponent <MeshFilter> ().mesh.CombineMeshes (col);
        chunk.GetComponent <MeshFilter> ().mesh.RecalculateBounds ();
        chunk.GetComponent <MeshFilter> ().mesh.RecalculateNormals ();
        chunk.GetComponent <MeshRenderer> ().material = material;
        return chunk;
    }
}
public struct ChunksGenerationJob : IJobParallelFor
{
    public static int ChunkSize;
    public static Mesh CubeMesh;
    public static Material CubeMaterial;
    public static Transform ChunkLoaderTransform;
    public NativeArray <Vector3> ChunksLocations;
    public NativeArray <CombineInstance> CombineInstances;

    public void Execute (int i)
    {
        ChunkLoad (ChunksLocations[i], i);
    }
    private void ChunkLoad (Vector3 chunkLocation, int index)
    {
        var num = new NativeArray<float> (ChunkSize * ChunkSize * ChunkSize, Allocator.Persistent);
        var chunkGeneration = new ChunkGenerationJob
        {
            ChunkLocation = chunkLocation,
            CombinedNum = num
        };
        var o = chunkGeneration.Schedule (ChunkSize * ChunkSize * ChunkSize, 0);
        o.Complete ();
        //var colMesh = new CombineInstance [ChunkSize * ChunkSize * ChunkSize];
        for (int i = 0; i < ChunkSize * ChunkSize * ChunkSize; i++)
        {
            if (chunkGeneration.CombinedNum [i] == 1)
            {
                var u = new CombineInstance ();
                u.mesh = CubeMesh;
                var ju = new GameObject();
                ju.transform.position = (new Vector3 (((i - (i % ChunkSize) - (((i - (i % ChunkSize)) / ChunkSize) % ChunkSize)) / ChunkSize / ChunkSize), (((i - (i % ChunkSize)) / ChunkSize) % ChunkSize), (i % ChunkSize)) + chunkLocation*ChunkSize);
                u.transform = ju.transform.localToWorldMatrix;
                GameObject.Destroy (ju);
                CombineInstances[index * ChunkSize * ChunkSize * ChunkSize + i] = u;
            }
        }
        num.Dispose ();
    }
}