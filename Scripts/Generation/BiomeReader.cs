using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeReader : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField]
    private Gradient gradient;

    void Start()
    {
            StartCoroutine(Read());
    }

    IEnumerator Read()
    {
        while (true)
        {
            material.color = gradient.Evaluate (Mathf.PerlinNoise ((transform.position.x + .01f) * PointFabric.BiomeSize, (transform.position.z+ .01f) * PointFabric.BiomeSize));
            Debug.Log (Mathf.PerlinNoise ((transform.position.x + .01f) * PointFabric.BiomeSize, (transform.position.z+ .01f) * PointFabric.BiomeSize));
            yield return new WaitForFixedUpdate ();
            yield return new WaitForFixedUpdate ();
        }
    }
}
