using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravitation : MonoBehaviour
{
    [SerializeField]
    float gravitationAcceleration;
    [SerializeField]
    Transform planet;
    Rigidbody rb;
    Transform tr;
    Vector3 vector;

    private void Start() 
    {
        rb = this.GetComponent <Rigidbody> ();
        tr = this.GetComponent <Transform> ();
    }
    private void FixedUpdate() 
    {
        vector = (planet.position - tr.position).normalized * gravitationAcceleration * Time.fixedDeltaTime;
        rb.velocity += vector;
    }
}
