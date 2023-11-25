using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class LineRenderObj : MonoBehaviour
{
    #region Public VARs
    [HeaderPlus(" ", "- Points -", (int)HeaderPlusColor.yellow)]
    [SerializeField] Transform StartPoint;
    [SerializeField] Transform FinalPoint;
    [SerializeField] Vector3 offSetY;
    private LineRenderer lineRenderer;
    #endregion

    void Start()
    {
        // Create or find Line Renderer in GameObject
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }  
    }

    void Update()
    {
        //Calc
        if (StartPoint != null && FinalPoint != null)
        {
            // Calculates the distance between two points
            float distance = Vector2.Distance(StartPoint.position, FinalPoint.position);

            lineRenderer.SetPosition(0, StartPoint.position);
            lineRenderer.SetPosition(1, FinalPoint.position + offSetY);

            Debug.Log("Dist: " + distance.ToString("F2"));
        }

        else
        {
            Debug.LogWarning("Start and end points is OFF.");
            gameObject.SetActive(false);
        }
    }
}
