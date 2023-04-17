using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.currentResolution.width < 1024)
        {
            Screen.SetResolution(1024, 768, Screen.fullScreen);
        }
        else
        {
            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
        }
    }
}
