using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChangeResolution : MonoBehaviour
{
    Resolution[] resolutions;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    public bool fullscreen;
    [SerializeField] Toggle toggle;

    int filteredIndexDistance = 0;
    private void Start()
    {
        fullscreen = Screen.fullScreen;
        resolutionDropdown.ClearOptions();
        toggle.isOn = fullscreen;
        resolutions = Screen.resolutions;
        List<string> _options = new();

        int currentResolutionIndex = 0;

        int i = 0;
        for (int j = 0; j < resolutions.Length; j++)
        {
            if (resolutions[j].width >= 1024 && resolutions[j].height >= 768)
            {
                string option = resolutions[j].width + " x " + resolutions[j].height;
                _options.Add(option);
                
                if (resolutions[j].width == Screen.width &&
                resolutions[j].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
                filteredIndexDistance++; i++;
            }
        }
        resolutionDropdown.AddOptions(_options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        if(Screen.width <= 1024 && Screen.height <= 768)
            Screen.SetResolution(1024, 768, Screen.fullScreen);
        else
        {
            Resolution resolution = resolutions[resolutionIndex + resolutions.Length - filteredIndexDistance];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }

    public void ToggleFullscreen(bool f)
    {
        int fInt;

        if (toggle.isOn)
            fInt = 1; 
        else
            fInt = 0;
        //print("toggle.isOn: " + toggle.isOn);
        //print("fInt: " + fInt);
        PlayerPrefs.SetFloat("fullscreen", fInt);

        Screen.fullScreen = f;
        //f = !f;

    }
}
