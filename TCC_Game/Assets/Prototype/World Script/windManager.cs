using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class windManager : MonoBehaviour
{
    GameObject wind;
    [SerializeField] Color colorGizmo = Color.red;
    [Space(5)]

    [Header(" // ")]
    public Vector3 position;
    [Space(5)]
    
    [Header(" // ")]
    [Range(0f, 5f)] public float intensity;
    [Space(5)]

    [Header(" UI ")]
    [SerializeField] GameObject image;

    private void Start() {
        //Debug.Log("Position  [ " + position  + "  ]");
        //Debug.Log("Intensity [ " + intensity + " ]");

        
    }

    public void FixedUpdate() {
        //Debug.Log("Position  [ " + position  + " ]");
        //Debug.Log("Intensity [ " + intensity + " ]");
        position = transform.position;

        //UI
        image.transform.Rotate(0, 0, position.x);
    }

    private void OnDrawGizmos() {
        Gizmos.color = colorGizmo;
    
        //Sempre manda para o centro da camera
        Gizmos.DrawLine(transform.position, position);
    }
}
