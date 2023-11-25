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

    [SerializeField] Slider sldMaster;
    [SerializeField] Slider sldMusic;
    [SerializeField] Slider sldSound;
    [SerializeField] Slider sldUI;

    [SerializeField] Slider sldUiScale;
    [Space(10)]
    public string[] soundKeys = {"sldMaster", "sldSound", "sldMusic", "sldUI" };
    public float[] defaultSoundValues = {1, 1, 0.8f, 0.95f };
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

        tggFullscreen.GetComponent<ChangeResolution>().ToggleFullscreen( IntToBool(PlayerPrefs.GetInt("fullscreen")) );
    }

    private void Start()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("volMaster"));
        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("volMusic"));
        audioMixer.SetFloat("Sound", PlayerPrefs.GetFloat("volSound"));
        audioMixer.SetFloat("UI", PlayerPrefs.GetFloat("volUI"));
    }

    public int LoadPrefs(string key, int i)
    {
        return 0;
    }

    int BoolToInt(bool b)
    {
        if (b) return 1;
        else return 0;
    }
    bool IntToBool(int i)
    {
        if (i == 1) return true;
        else return false;
    }
}
