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
    private ChunkFabric chunkFabric = new ChunkFabric ();
    [SerializeField]
    private float chunkReloadTime;
    [SerializeField]
    private Material terrainMaterial;

    private void Start() 
    {
        player = this.transform;
        chunk = new Chunk ();
        ter = new GameObject [chunkSpawnRadius*2, chunkSpawnRadius*2, chunkSpawnRadius*2];
        for (int i = 0; i < chunkSpawnRadius * 2; i++)
        {
            for (int j = 0; j < chunkSpawnRadius * 2; j++)
            {
                for (int k = 0; k < chunkSpawnRadius * 2; k++)
                {

                    ter[i,j,k] = new GameObject ("Name", typeof (MeshFilter), typeof (MeshRenderer), typeof (MeshCollider));
                    ter[i,j,k].GetComponent<MeshRenderer>().material = terrainMaterial;
                }
            }
        }
        StartCoroutine (Reload());
    }

    private Vector3Int PlayerPosition ()
    {
        return Vector3Int.RoundToInt(player.position/Chunk.Size/Chunk.PointSize);
    }

    IEnumerator Reload ()
    {
        while (true)
        {
            if (PlayerPosition () != playerPosition)
            {
                playerPosition = PlayerPosition ();
                for (int i = 0; i < chunkSpawnRadius * 2; i++)
                {
                    for (int j = 0; j < chunkSpawnRadius * 2; j++)
                    {
                        for (int k = 0; k < chunkSpawnRadius * 2; k++)
                        {
                            var l = new Stopwatch ();
                            chunk = chunkFabric.Create (new Vector3Int (playerPosition.x-chunkSpawnRadius+i, playerPosition.y-chunkSpawnRadius+j, playerPosition.z-chunkSpawnRadius+k));
                            l.Start ();
                            chunkFabric.GeneratePoints (ref chunk, .03f);
                            l.Stop();
                            UnityEngine.Debug.Log (l.ElapsedMilliseconds.ToString ());
                            l.Start ();
                            chunkFabric.GenerateMesh (ref chunk);
                            l.Stop();
                            UnityEngine.Debug.Log (l.ElapsedMilliseconds.ToString ());
                            ter[i,j,k].GetComponent<MeshFilter>().mesh = chunk.TerrainMesh;
                            ter[i,j,k].GetComponent<MeshFilter>().mesh.RecalculateBounds ();
                            ter[i,j,k].GetComponent<MeshFilter>().mesh.RecalculateNormals ();
                            ter[i,j,k].GetComponent<MeshCollider>().sharedMesh = chunk.TerrainMesh;
                            yield return new WaitForEndOfFrame ();
                        }
                    }
                }
            }
            yield return new WaitForSeconds (chunkReloadTime);
        }
    }
}
