using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChangeSliderLabel : MonoBehaviour
{

    TextMeshProUGUI inputLabel;
    Slider sld;
    public string unit = "%";

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

}
