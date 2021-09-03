using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Transform tr;
    Transform bodyTr;
    Rigidbody rb;
    [SerializeField]
    public float speed;
    Vector2 velocity;
    bool trigger;
    private float speedAdd;

    void Start()
    {
        tr = this.GetComponent <Transform> ();
        bodyTr = tr.GetChild (0);
        rb = this.GetComponent <Rigidbody> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (trigger)
        // {
            velocity = new Vector2 (Input.GetAxis("Vertical"), Input.GetAxis ("Horizontal")).normalized;
            rb.velocity += new Vector3 ( bodyTr.forward.x * velocity.x + bodyTr.right.x * velocity.y, 
                                        bodyTr.forward.y * velocity.x + bodyTr.right.y * velocity.y, 
                                        bodyTr.forward.z * velocity.x + bodyTr.right.z * velocity.y) * (speed + speedAdd) * Time.fixedDeltaTime;
        //}
        // if (rb.velocity.magnitude > 10)
        // {
        //     rb.velocity = rb.velocity.normalized * 10;
        // }
    }
    private void Update() 
    {
        if (Input.GetKeyDown (KeyCode.LeftShift))
        {
            speedAdd = 10;
        }
        else if (Input.GetKeyDown (KeyCode.LeftShift))
        {
            speedAdd = -5;
        }
        else if (Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.LeftShift))
        {
            speedAdd = 0;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        trigger = true;
    }
    private void OnTriggerStay(Collider other) 
    {
        if (!trigger)
        {
            trigger = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        trigger = false;
    }
}
