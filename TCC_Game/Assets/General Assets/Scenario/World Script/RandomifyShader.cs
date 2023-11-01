using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomifyShader : MonoBehaviour
{
    private Renderer rend;
    void Start()
    {
        if (rend == null)
            rend = GetComponent<Renderer>();

        rend.material.SetFloat("_Random", Random.Range(0,10));
    }
}
