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

    int filteredIndexDistance = 0;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

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
                    SetResolution(currentResolutionIndex);
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

            Debug.Log($"index resolucao atual: {resolutionIndex}" +
                            $"\n{resolution.width}, {resolution.height}");
        }
    }

    public void ToggleFullscreen(bool f)
    {
        f = !f;
        Screen.fullScreen = f;
    }
}
