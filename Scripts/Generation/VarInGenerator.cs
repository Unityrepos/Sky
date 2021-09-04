using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarInGenerator : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Transform>().parent.GetComponent <MainGenerationVar>().AddPlanetPart (this.GetComponent <MeshFilter>().mesh, this.GetComponent<MeshCollider>());
    }
}
