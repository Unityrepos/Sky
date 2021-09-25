using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ChunkRadiusSpawn : MonoBehaviour
{
    [SerializeField]
    private int chunkSpawnRadius;
    private Transform player;
    private Vector3Int playerPosition;
    private Chunk chunk;
    private GameObject[,,] ter;
    private Mesh[,,] terMesh;
    private ChunkFabric chunkFabric = new ChunkFabric();
    [SerializeField]
    private float chunkReloadTime;
    [SerializeField]
    private Material terrainMaterial;
    private Chunks chunks = new Chunks();

    private void Start()
    {
        MathU.SeedGenerator (42);
        MathU.NoiseGenerator (256);
        player = this.transform;
        chunk = new Chunk();
        ter = new GameObject[chunkSpawnRadius * 2, chunkSpawnRadius * 2, chunkSpawnRadius * 2];
        terMesh = new Mesh[chunkSpawnRadius * 2, chunkSpawnRadius * 2, chunkSpawnRadius * 2];
        for (int i = 0; i < chunkSpawnRadius * 2; i++)
        {
            for (int j = 0; j < chunkSpawnRadius * 2; j++)
            {
                for (int k = 0; k < chunkSpawnRadius * 2; k++)
                {

                    ter[i, j, k] = new GameObject("Name", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                    ter[i, j, k].GetComponent<MeshRenderer>().material = terrainMaterial;
                }
            }
        }
        MarchingCubesSmoothTriangles.Limit = 0.5f;
        StartCoroutine(Reload());
    }

    private Vector3Int PlayerPosition()
    {
        return Vector3Int.RoundToInt(player.position / Chunk.Size / Chunk.PointSize);
    }

    IEnumerator Reload()
    {
        while (true)
        {
            if (PlayerPosition() != playerPosition)
            {
                playerPosition = PlayerPosition();
                for (int i = 0; i < chunkSpawnRadius * 2; i++)
                {
                    for (int j = 0; j < chunkSpawnRadius * 2; j++)
                    {
                        for (int k = 0; k < chunkSpawnRadius * 2; k++)
                        {
                            if (!chunks.Contains(new Vector3Int(playerPosition.x - chunkSpawnRadius + i, playerPosition.y - chunkSpawnRadius + j, playerPosition.z - chunkSpawnRadius + k)))
                            {
                                var l = new Stopwatch();
                                l.Start();
                                chunk = chunkFabric.Create(new Vector3Int(playerPosition.x - chunkSpawnRadius + i, playerPosition.y - chunkSpawnRadius + j, playerPosition.z - chunkSpawnRadius + k));
                                chunkFabric.GeneratePoints(ref chunk, .005f);
                                if (!chunk.IsEmpty)
                                {
                                    chunkFabric.GenerateMesh(ref chunk);
                                    yield return new WaitForEndOfFrame();
                                }
                                chunks.AddChunk(new Vector3Int(playerPosition.x - chunkSpawnRadius + i, playerPosition.y - chunkSpawnRadius + j, playerPosition.z - chunkSpawnRadius + k), chunk);
                                l.Stop();
                                UnityEngine.Debug.Log(l.ElapsedMilliseconds.ToString());
                            }
                            else
                            {
                                chunks.Chunk(new Vector3Int(playerPosition.x - chunkSpawnRadius + i, playerPosition.y - chunkSpawnRadius + j, playerPosition.z - chunkSpawnRadius + k), ref chunk);
                            }
                            terMesh[i, j, k] = chunk.TerrainMesh;
                        }
                    }
                }
                for (int i = 0; i < chunkSpawnRadius * 2; i++)
                {
                    for (int j = 0; j < chunkSpawnRadius * 2; j++)
                    {
                        for (int k = 0; k < chunkSpawnRadius * 2; k++)
                        {
                            ter[i, j, k].GetComponent<MeshFilter>().mesh = terMesh[i, j, k];
                            ter[i, j, k].GetComponent<MeshFilter>().mesh.RecalculateBounds();
                            ter[i, j, k].GetComponent<MeshFilter>().mesh.RecalculateNormals();
                            ter[i, j, k].GetComponent<MeshCollider>().sharedMesh = terMesh[i, j, k];
                        }
                    }
                }
            }
            yield return new WaitForSeconds(chunkReloadTime);
        }
    }
}
