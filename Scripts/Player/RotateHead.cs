using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHead : MonoBehaviour
{
    private Transform tr;
    [SerializeField]
    private float multiplier;
    float angle;

    private void Start() 
    {
        tr = this.GetComponent <Transform> ();
    }
    private void Update() 
    {
        angle += Input.GetAxis ("Mouse Y") * Time.deltaTime * multiplier;
        angle = Mathf.Clamp (angle, -60, 60);
        tr.localEulerAngles = new Vector3 (-angle, 0, 0);
    }
}
