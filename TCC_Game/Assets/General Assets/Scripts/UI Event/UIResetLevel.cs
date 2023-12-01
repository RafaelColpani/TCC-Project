using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KevinCastejon.MoreAttributes;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIResetLevel : MonoBehaviour
{
    [HeaderPlus(" ", "- UI -", (int)HeaderPlusColor.cyan)]
    [SerializeField] Slider resetSlider;

    [HeaderPlus(" ", "- SLIDER -", (int)HeaderPlusColor.cyan)]
    [SerializeField] float fillSpeed = 0.5f;

    private bool fillSlider;

    void Start()
    {
        resetSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (fillSlider)
        {
            FillresetSlider();
        }

        else if (!fillSlider && resetSlider.value > 0)
        {
            EmptyresetSlider();
        }
    }

    public void FillresetSlider()
    {
        if (!fillSlider)
            fillSlider = true;

        if (resetSlider.value < 1f)
        {
            resetSlider.value += fillSpeed * Time.deltaTime;
        }

        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void EmptyresetSlider()
    {
        fillSlider = false;

        if (resetSlider.value > 0f)
        {
            resetSlider.value -= fillSpeed * Time.deltaTime;
        }
    }
}
