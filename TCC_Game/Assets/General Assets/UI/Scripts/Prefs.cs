using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static ChangeSliderLabel;

public class Prefs : MonoBehaviour
{

    bool defaultPrefs = false;

    [SerializeField] AudioMixer audioMixer;
    #region Settings

    string resolution;
    int fullscreen; // 1 = true; 0 = false;

    float volMaster;
    float volMusic;
    float volSound;
    float volUI;

    float uiScale;

    //[SerializeField] Slider resolution;
    [SerializeField] Toggle tggFullscreen;

    [SerializeField] Slider sldUiScale;
    [Space(10)]
    public string[] soundKeys = {"sldMaster", "sldMusic", "sldSound", "sldUI" };
    public float[] defaultSoundValues = {1, 1, 1, 1};
    [Space(10)]
    public float canvasScaleValue = 1;

    #endregion

    private void Awake()
    {
        for (int i = 0; i < defaultSoundValues.Length; i++)
        {
            if (!PlayerPrefs.HasKey(soundKeys[i]))
                PlayerPrefs.SetFloat(soundKeys[i], defaultSoundValues[i]);
        }

        if (!PlayerPrefs.HasKey("canvasScaleValue"))
            PlayerPrefs.SetFloat("canvasScaleValue", 1);

        //tggFullscreen.GetComponent<ChangeResolution>().ToggleFullscreen( IntToBool(PlayerPrefs.GetInt("fullscreen")) );
    }

    private void Start()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("volMaster"));
        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("volMusic"));
        audioMixer.SetFloat("Sound", PlayerPrefs.GetFloat("volSound"));
        audioMixer.SetFloat("UI", PlayerPrefs.GetFloat("volUI"));

        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("volMusic"));
        audioMixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("sldMusic")));
    }

    int BoolToInt(bool b)
    {
        if (b) return 1;
        else return 0;
    }
    bool IntToBool(int i)
    {
        if (i == 1)
        {
            print("inttobool: " + "1 true");
            return true;
        }
        else
        {
            print("inttobool: " + "0 false");
            return false;
        }
    }
}
