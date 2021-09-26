using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeReader : MonoBehaviour
{
    public Color material;
    public Color mTwo;
    public Material materia;

    void Start()
    {
    }
    private void Update() 
    {
        if (Input.GetKeyDown (KeyCode.L))
        {
            StartCoroutine(Read());
        }
    }

    IEnumerator Read()
    {
        mTwo = materia.color;
        for (int i = 1; i < 31; i++)
        {
            materia.color = (Color.Lerp (mTwo, material, (float)i/30));
            yield return new WaitForFixedUpdate ();
        }
    }
}
