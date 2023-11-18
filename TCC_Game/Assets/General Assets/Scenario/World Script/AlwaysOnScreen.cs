using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysOnScreen : MonoBehaviour
{
    [SerializeField] Transform cam;


    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(cam.position.x, cam.position.y, 0);
    }
}
