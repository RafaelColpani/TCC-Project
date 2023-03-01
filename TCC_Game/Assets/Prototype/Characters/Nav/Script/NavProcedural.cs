using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavProcedural : MonoBehaviour
{
    [SerializeField] int length;
    [SerializeField] LineRenderer lineRender;
    [SerializeField] Vector3[] segment;
    [SerializeField] Vector3[] segmentV;

    [SerializeField] Transform targetDir;
    [SerializeField] float targetDist;
    [SerializeField] float smoothSpeed;

    //Collider



    private void Start() {
        lineRender.positionCount = length;
        segment = new Vector3[length];
        segmentV = new Vector3[length];

    }

    private void Update() {
        segment[0] = targetDir.position;
        for(int i = 1; i < segment.Length; i++){
            segment[i] = Vector3.SmoothDamp(segment[i], segment[i - 1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed);
        }
        
        lineRender.SetPositions(segment);
    }

    
}
