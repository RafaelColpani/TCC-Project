using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [HideInInspector] public Transform obj;
    [HideInInspector] public Vector3 offset;
    [SerializeField] AnimationCurve curve;
    Vector2 size;

    private void Start()
    {
        size = this.gameObject.transform.localScale;
        StartCoroutine(scaleUp());
    }

    private IEnumerator scaleUp()
    {

        float scaleDuration = 0.2f;
        float scaleTimer = 0;

        while (scaleTimer < scaleDuration)
        {
            float t = scaleTimer / scaleDuration;
            float scale = curve.Evaluate(t);
            transform.localScale = size * scale;
            scaleTimer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = size;
    }

    void Update()
    {
        this.gameObject.transform.position = obj.position + offset;
    }
}
