using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineWind : MonoBehaviour
{
    
    public windManager script;
    public float a;
    public Vector3 b;

    private void Start() 
    {
        
       
    } 

    private void Update() 
    {
        a = script.intensity;
        b = script.position;

        print(a);
       print(b);

    }
}
