using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Runtime.InteropServices;
using MoreMountains.Tools;
using UnityEngine.Audio;
using UnityEngine.Assertions.Must;

public class ChangeSliderLabel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{

    TextMeshProUGUI inputLabel;
    Slider sld;

    [Tooltip("By default '%' will be the default unit, otherwise, by default it'll be used on sound related UI sliders.")]
    public string unit = "%";
    
    [SerializeField] Canvas canvas;

    [Tooltip("Controls ScaleFactor parameter of Canvas")]
    float canvasScaleValue = 1;

    [Space(20)]

    public AudioMixer audioMixer;

    [Space(15)]

    [SerializeField]
    private MixerType mixerType;
    public enum MixerType
    {
        Master,
        Sound,
        Music,
        UI
    }

    private void Start()
    {
        sld = GetComponent<Slider>();

        //if (mixerType == MixerType.Music)
        //{
        //    audioMixer.GetFloat("Music", out float f);
        //    sld.value = f;
        //}



        inputLabel = GetComponentInChildren<TextMeshProUGUI>();

        if(audioMixer != null)
        {
            audioMixer.GetFloat("Master", out float val);
            audioMixer.SetFloat("Master", val);
            print("master float: " + val);

            float labelValue = 100 * sld.value;
            decimal decimalValue = System.Math.Round((decimal)labelValue, 0);

            inputLabel.text = $"{decimalValue}{unit}";

            //ChangeValue();
        }
    }

    void ChangeAudioVolume()
    {
        switch (mixerType)
        {
            case MixerType.Master:
                {
                audioMixer.GetFloat("Master", out float val);
                audioMixer.SetFloat("Master", Mathf.Log10(sld.value) * 20);
                    print("master float: " + Mathf.Log10(sld.value) * 20);
                }
                break;

            case MixerType.Sound:
                audioMixer.SetFloat("Sound", Mathf.Log10(sld.value) * 20);
                audioMixer.GetFloat("Footstep", out float f);
                audioMixer.SetFloat("Footstep", f - Mathf.Log10(sld.value) * 20);
                break;

            case MixerType.Music:

                audioMixer.SetFloat("Music", Mathf.Log10(sld.value) * 20);
                break;

            case MixerType.UI:
                audioMixer.SetFloat("UI", Mathf.Log10(sld.value) * 20);
                break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// Audio Volume Changer
    /// </summary>
    public void ChangeValue()
    {
        // muda o volume do mixer
        ChangeAudioVolume();

        // modifica a visualizacao do valor na UI
        float labelValue = 100 * sld.value;
        decimal decimalValue = System.Math.Round((decimal)labelValue, 0);

        inputLabel.text = $"{decimalValue}{unit}";
    }

    /// <summary>
    /// Canvas Scale Changer
    /// </summary>
    public void ChangeScaleValue(float value)
    {
        float newValue = Mathf.Round(value * 10f)/10f;
        sld.value = newValue;
        newValue = sld.value;
        inputLabel.text = $"{newValue/*.ToString("F1")*/}{unit}";
        canvasScaleValue = newValue;
    }

    void SetCanvasScaleValue()
    {
        canvas.GetComponent<CanvasScaler>().scaleFactor = canvasScaleValue;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(unit == "x")
        {
            ChangeScaleValue(canvasScaleValue);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unit == "x")
        {
            ChangeScaleValue(canvasScaleValue);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (unit == "x")
            SetCanvasScaleValue();
    }
}
