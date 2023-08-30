using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform defaultPosition;
    Transform defaultParent;
    [SerializeField] RectTransform outTransform;

    bool isScaling = false;
    void Start()
    {
        defaultParent = transform.parent;
        defaultPosition = GetComponent<RectTransform>();
    }


    void LateUpdate()
    {
        if (isScaling)
        {
            transform.SetParent(outTransform);
            //transform.position = outTransform.position;
            //GetComponent<RectTransform>().anchoredPosition = 
        }
        else
        {
            transform.SetParent(defaultParent);
            //transform.position = defaultPosition.position;
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
