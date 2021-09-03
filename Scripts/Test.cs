using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent <Transform> ().Translate (0,new Vector2 (this.GetComponent <Transform> ().position.x, this.GetComponent <Transform> ().position.z).Perlin (),0);
    }

}
