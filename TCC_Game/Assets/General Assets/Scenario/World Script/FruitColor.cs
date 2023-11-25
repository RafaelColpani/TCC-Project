using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitColor : MonoBehaviour
{
    [SerializeField] Color color;
    Renderer rend;
    void Start()
    {
        rend = this.GetComponent<Renderer>();
        rend.material.SetColor("_Color", color);
    }
}
