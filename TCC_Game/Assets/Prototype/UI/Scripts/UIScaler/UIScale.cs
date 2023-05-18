using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform defaultPosition;
    Transform defaultParent;
    RectTransform outTransform;

    bool isScaling = false;
    void Start()
    {
        defaultParent = transform.parent;
        defaultPosition = GetComponent<RectTransform>();
    }


    void Update()
    {
        if (isScaling)
        {
            transform.SetParent(outTransform.parent);
        }
        else
        {
            transform.SetParent(defaultParent);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isScaling = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isScaling = false;
    }
}
