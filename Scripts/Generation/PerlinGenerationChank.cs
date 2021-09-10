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
    float scale;
    [SerializeField]
    MeshFilter cube;
    Mesh cubeMesh;
    MeshFilter worldMesh;
    void Start()
    {
        worldMesh = this.GetComponent <MeshFilter> ();

        cubeMesh = cube.mesh;
        MathU.SeedGenerator (42);
        MathU.NoiseGenerator (256);

        ChunkGenerationJob.ChunkSize = chunkSize;
        ChunkGenerationJob.Scale = scale;
        ChunkLoad (new Vector3 (0, 0, 0));

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
        worldMesh.mesh.CombineMeshes (colMesh);
        worldMesh.mesh.RecalculateNormals ();
        worldMesh.mesh.RecalculateBounds ();
    }
}
