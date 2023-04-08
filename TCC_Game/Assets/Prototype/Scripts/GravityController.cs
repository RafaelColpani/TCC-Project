using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [Tooltip("Sets the value of gravity. Standard is 9.81.")]
    [SerializeField] float gravity = 9.81f;

    Vector3 velocity;

    private void FixedUpdate()
    {
        
    }
}
