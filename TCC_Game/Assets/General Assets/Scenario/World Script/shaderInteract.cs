using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderInteract : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
       // player = GameObject.FindGameObjectWithTag("Player");
        
        Vector3 playerPos = player.GetComponent<Transform>().position;
        material.SetVector("_Player", playerPos);
    }
}
