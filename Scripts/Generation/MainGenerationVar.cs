using System.Collections;//
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class MainGenerationVar : MonoBehaviour
{
    [SerializeField]
    private int planetPartsCount;
    [SerializeField]
    private float radius;
    private int planetParts = 0;
    private float[][] heights;
    private Vector3[][] vertices;
    private Mesh[] meshes;
    private MeshCollider[] meshColliders;
    private bool allParts;

    private void Awake() 
    {
        heights = new float [planetPartsCount][];
        vertices = new Vector3 [planetPartsCount][];
        meshes = new Mesh [planetPartsCount];
        meshColliders = new MeshCollider [planetPartsCount];
    }

    public int AddPlanetPart (Mesh mesh, MeshCollider mc)
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
        return planetParts;
    }
}
