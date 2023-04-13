using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChangeResolution : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions;

    float currentRefreshRate;
    int currentResolutionIndex = 0;

    public bool fullscreen;

    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate && resolutions[i].width >= 1024 && resolutions[i].height >= 768)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = $"{filteredResolutions[i].width}x{filteredResolutions[i].height}"; // + $"{filteredResolutions[i].refreshRate} Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
    }

    public void ToggleFullscreen(bool f)
    {
        f = !f;
        fullscreen = f;
        Screen.SetResolution(Screen.width, Screen.height, fullscreen);
    }
}
