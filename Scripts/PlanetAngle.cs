using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAngle : MonoBehaviour
{
    [SerializeField]
    Transform planet;
    Transform tr;

    private void Start() 
    {
        tr = this.GetComponent <Transform> ();
    }
    private void FixedUpdate() 
    {
        tr.up = tr.position - planet.position;
    }
}
