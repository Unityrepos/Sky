using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarInPart : MonoBehaviour
{
    int id;

    void Start()
    {
        id = this.GetComponent<Transform>().parent.GetComponent <MainGenerationVar>().AddPlanetPart (this.GetComponent <MeshFilter>().mesh, this.GetComponent<MeshCollider>());
    }
}