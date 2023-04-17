using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followObject : MonoBehaviour
{
    [SerializeField] SphereCollider sphere;
    [SerializeField] Transform body;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        sphere.center = body.transform.position;
    }
}
