using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private Rigidbody playerRigidbody;
    private Transform playerTransform;

    private AnimationCurve xCurve;
    private AnimationCurve yCurve;
    private AnimationCurve zCurve;

    public PlayerMovement (GameObject player)
    {
        this.Player = player;
    }

    public GameObject Player
    {
        set
        {
            playerTransform = value.transform;
            playerRigidbody = value.GetComponent <Rigidbody> ();
        }
    }
    public Vector3 AxisVelocity
    {
        get
        {
            var i = new Vector3((float.IsInfinity(playerRigidbody.velocity.x/playerTransform.right.x)   ? 0 : playerRigidbody.velocity.x/playerTransform.right.x)   +
                                (float.IsInfinity(playerRigidbody.velocity.z/playerTransform.forward.x) ? 0 : playerRigidbody.velocity.z/playerTransform.forward.x) +
                                (float.IsInfinity(playerRigidbody.velocity.y/playerTransform.up.x)      ? 0 : playerRigidbody.velocity.y/playerTransform.up.x),
                                (float.IsInfinity(playerRigidbody.velocity.x/playerTransform.right.y)   ? 0 : playerRigidbody.velocity.x/playerTransform.right.y)   +
                                (float.IsInfinity(playerRigidbody.velocity.z/playerTransform.forward.y) ? 0 : playerRigidbody.velocity.z/playerTransform.forward.y) +
                                (float.IsInfinity(playerRigidbody.velocity.y/playerTransform.up.y)      ? 0 : playerRigidbody.velocity.y/playerTransform.up.y),
                                (float.IsInfinity(playerRigidbody.velocity.x/playerTransform.right.z)   ? 0 : playerRigidbody.velocity.x/playerTransform.right.z)   +
                                (float.IsInfinity(playerRigidbody.velocity.z/playerTransform.forward.z) ? 0 : playerRigidbody.velocity.z/playerTransform.forward.z) +
                                (float.IsInfinity(playerRigidbody.velocity.y/playerTransform.up.z)      ? 0 : playerRigidbody.velocity.y/playerTransform.up.z));
            return i.normalized*playerRigidbody.velocity.magnitude;
        }
    }

    public Vector3 Acceleration
    {
        get
        {
            var j = AxisVelocity;
            var i = new Vector3 (xCurve.Evaluate (j.x), yCurve.Evaluate (j.y), zCurve.Evaluate (j.z));
            i = new Vector3(i.x * playerTransform.right.x + i.y * playerTransform.up.x + i.z * playerTransform.forward.x, 
                            i.x * playerTransform.right.y + i.y * playerTransform.up.y + i.z * playerTransform.forward.y, 
                            i.x * playerTransform.right.z + i.y * playerTransform.up.z + i.z * playerTransform.forward.z);
            return i;
        }
    }
    public Vector3 AccelerationWithVector (Vector3 vector)
    {
        var j = AxisVelocity;
        var i = new Vector3(xCurve.Evaluate (j.x + vector.x) * playerTransform.right.x + yCurve.Evaluate (j.y + vector.y) * playerTransform.up.x + zCurve.Evaluate (j.z + vector.z) * playerTransform.forward.x, 
                        xCurve.Evaluate (j.x + vector.x) * playerTransform.right.y + yCurve.Evaluate (j.y + vector.y) * playerTransform.up.y + zCurve.Evaluate (j.z + vector.z) * playerTransform.forward.y, 
                        xCurve.Evaluate (j.x + vector.x) * playerTransform.right.z + yCurve.Evaluate (j.y + vector.y) * playerTransform.up.z + zCurve.Evaluate (j.z + vector.z) * playerTransform.forward.z);
        return i;
    }

    public AnimationCurve XCurve { get => xCurve; set => xCurve = value; }
    public AnimationCurve YCurve { get => yCurve; set => yCurve = value; }
    public AnimationCurve ZCurve { get => zCurve; set => zCurve = value; }
    public Transform PlayerTransform { get => playerTransform; set => playerTransform = value; }
    public Rigidbody PlayerRigidbody { get => playerRigidbody; set => playerRigidbody = value; }
}
