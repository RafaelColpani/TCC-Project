using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpDown : MonoBehaviour
{
    public float minScale = 1.0f;
    public float maxScale = 1.5f;
    public float scaleSpeed = 1.0f;

    private bool scalingUp = true;
    private float currentScale;

    private void Start()
    {
        currentScale = transform.localScale.x;
    }

    private void Update()
    {
        float newScale = currentScale + (scalingUp ? scaleSpeed : -scaleSpeed) * Time.deltaTime;
        newScale = Mathf.Clamp(newScale, minScale, maxScale);

        if (newScale == minScale || newScale == maxScale)
        {
            scalingUp = !scalingUp;
        }

        transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        currentScale = newScale;
    }
}
