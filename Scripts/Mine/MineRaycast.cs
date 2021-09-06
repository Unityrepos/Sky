using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineRaycast : MonoBehaviour
{
    Camera camera;
    Ray ray;
    RaycastHit hit;
    MineEvent mineEvent;
    
    void Start()
    {
        camera = this.GetComponent <Camera> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown (0))
        {
            ray = camera.ScreenPointToRay (new Vector3 (camera.pixelWidth/2, camera.pixelHeight/2, 0));
            if (Physics.Raycast (ray, out hit))
            {
                if ((mineEvent = hit.transform.GetComponent<MineEvent>()) != null)
                {
                    mineEvent.Mine (1, hit.point, 7);
                }
            }
        }
    }
}
