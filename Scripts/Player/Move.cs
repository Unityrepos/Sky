using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Transform tr;
    Transform bodyTr;
    Rigidbody rb;
    [SerializeField]
    float speed;
    Vector2 velocity;
    bool trigger;
    private float speedAdd = 1;
    [SerializeField]
    private AnimationCurve jumpCurve;
    [SerializeField]
    private int jumpFrames;
    [SerializeField]
    private float gravityForce;

    void Start()
    {
        tr = this.GetComponent <Transform> ();
        bodyTr = tr.GetChild (0);
        rb = this.GetComponent <Rigidbody> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = new Vector2 (Input.GetAxis("Vertical"), Input.GetAxis ("Horizontal")).normalized;
        rb.velocity += new Vector3 ( bodyTr.forward.x * velocity.x + bodyTr.right.x * velocity.y, 
                                    bodyTr.forward.y * velocity.x + bodyTr.right.y * velocity.y + gravityForce, 
                                    bodyTr.forward.z * velocity.x + bodyTr.right.z * velocity.y) * (speed * speedAdd) * Time.fixedDeltaTime;
    }
    private void Update() 
    {
        if (Input.GetKeyDown (KeyCode.LeftShift))
        {
            speedAdd = 2f;
        }
        else if (Input.GetKeyDown (KeyCode.LeftControl))
        {
            speedAdd = .5f;
        }
        else if (Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.LeftControl))
        {
            speedAdd = 1;
        }
        if (Input.GetKeyDown (KeyCode.Space) && trigger)
        {
            StartCoroutine (Jump ());
        }
    }
    private IEnumerator Jump ()
    {
        for (int i = 0; i < jumpFrames; i++)
        {
            yield return new WaitForFixedUpdate ();
            if (Input.GetKey (KeyCode.Space))
                rb.velocity += new Vector3 (0,jumpCurve.Evaluate (i*Time.fixedDeltaTime),0);
            else
                yield break;
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
