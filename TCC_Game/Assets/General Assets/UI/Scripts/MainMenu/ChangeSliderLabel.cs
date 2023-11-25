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
    private void Awake()
    {
        print("awakeeeeeee");
    }
    private void Start()
    {
        sld = GetComponent<Slider>();

        //if (mixerType == MixerType.Music)
        //{
        //    audioMixer.GetFloat("Music", out float f);
        //    sld.value = f;
        //}

        print("data loaded from player prefs: \n" +
            $"volMASTER: {PlayerPrefs.GetFloat("volMaster")}\n"+
            $"volMUSIC: {PlayerPrefs.GetFloat("volMusic")}\n" +
            $"volSOUND: {PlayerPrefs.GetFloat("volSound")}\n" +
            $"volUI: {PlayerPrefs.GetFloat("volUI")} \n" +

            $"sldMASTER: {PlayerPrefs.GetFloat("sldMaster")}\n" +
            $"sldMUSIC: {PlayerPrefs.GetFloat("sldMusic")}\n" +
            $"sldSOUND: {PlayerPrefs.GetFloat("sldSound")}\n" +
            $"sldUI: {PlayerPrefs.GetFloat("sldUI")} \n"
            );

        inputLabel = GetComponentInChildren<TextMeshProUGUI>();

        if(audioMixer != null)
        {
            
            //Debug.Log("val GETFLOAT: " + PlayerPrefs.GetFloat("volMaster"));
            //audioMixer.GetFloat("Master", out float val);
            //audioMixer.SetFloat("Master", val);
            //print("master float: " + val);


            //float labelValue = 100 * sld.value;
            //decimal decimalValue = System.Math.Round((decimal)labelValue, 0);
            float labelValue = 100 * sld.value; ;
            switch (mixerType)
            {
                case MixerType.Master:
                    labelValue = PlayerPrefs.GetFloat("sldMaster");
                    break;
                case MixerType.Sound:
                    labelValue = PlayerPrefs.GetFloat("sldSound");
                    break;
                case MixerType.Music:
                    labelValue = PlayerPrefs.GetFloat("sldMusic");
                    break;
                case MixerType.UI:
                    labelValue = PlayerPrefs.GetFloat("sldUI");
                    break;
            }

            sld.value = labelValue;

            decimal decimalValue = System.Math.Round((decimal)labelValue, 0);
            inputLabel.text = $"{decimalValue}{unit}";

            print($"labelvalue: {labelValue}; decimalvalue {decimalValue}");

            //PlayerPrefs.SetFloat("volMaster", val);
            //Debug.Log("val: " + val);

            ChangeValue();
        }
    }

    void ChangeAudioVolume()
    {
        switch (mixerType)
        {
            case MixerType.Master:
                {
                    audioMixer.SetFloat("Master", Mathf.Log10(sld.value) * 20);

                    //print("master float: " + Mathf.Log10(sld.value) * 20);
                    audioMixer.GetFloat("Master", out float val);
                    PlayerPrefs.SetFloat("volMaster", val);
                    PlayerPrefs.SetFloat("sldMaster", sld.value);
                    print("sldvalue:" + sld.value);
                }
                break;

            case MixerType.Sound:
                {
                    audioMixer.SetFloat("Sound", Mathf.Log10(sld.value) * 20);
                    audioMixer.GetFloat("Sound", out float val);

                    audioMixer.GetFloat("Footstep", out float f);
                    audioMixer.SetFloat("Footstep", f - Mathf.Log10(sld.value) * 20);

                    PlayerPrefs.SetFloat("volSound", val);
                    PlayerPrefs.SetFloat("sldSound", sld.value);
                }
                break;

            case MixerType.Music:
                {
                    audioMixer.SetFloat("Music", Mathf.Log10(sld.value) * 20);
                    audioMixer.GetFloat("Sound", out float val);
                    PlayerPrefs.SetFloat("volMusic", val);
                    PlayerPrefs.SetFloat("sldMusic", sld.value);
                }
                break;

            case MixerType.UI:
                {
                    audioMixer.SetFloat("UI", Mathf.Log10(sld.value) * 20);
                    audioMixer.GetFloat("UI", out float val);
                    PlayerPrefs.SetFloat("volUI", val);
                    PlayerPrefs.SetFloat("sldUI", sld.value);
                }
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
        print("CHANGE SLIDER VALUE: SLD.VALUE = " + sld.value);
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

        PlayerPrefs.SetFloat("canvasScale", canvasScaleValue);
        print("canvasScaleValue: " + canvasScaleValue);
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
