using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBody : MonoBehaviour
{
    private Transform tr;
    [SerializeField]
    private float multiplier;

    private void Start() 
    {
        tr = this.GetComponent <Transform> ();
    }
    private void Update() 
    {
        tr.Rotate (0, Input.GetAxis("Mouse X") * multiplier * Time.deltaTime, 0);
    }
}
