using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.InteropServices;

public class ChangeSliderLabel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{

    TextMeshProUGUI inputLabel;
    Slider sld;
    public string unit = "%";
    
    [SerializeField] Canvas canvas;
    float scaleValue = 1;

    private void Start()
    {
        sld = GetComponent<Slider>();
        inputLabel = GetComponentInChildren<TextMeshProUGUI>();
        ChangeValue();
    }

    public void ChangeValue()
    {
        inputLabel.text = $"{sld.value}{unit}";
    }

    public void ChangeScaleValue(float value)
    {
        inputLabel.text = $"{value.ToString("F1")}{unit}";
        scaleValue = sld.value;
    }

    void SetScaleValue()
    {
        canvas.GetComponent<CanvasScaler>().scaleFactor = scaleValue;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(unit == "x")
        {
            ChangeScaleValue(scaleValue);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unit == "x")
        {
            ChangeScaleValue(scaleValue);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (unit == "x")
            SetScaleValue();
    }
}
