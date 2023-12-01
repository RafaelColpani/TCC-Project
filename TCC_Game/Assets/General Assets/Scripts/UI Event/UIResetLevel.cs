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

    void Start()
    {
        resetSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FillresetSlider();
        }

        else
        {
            EmptyresetSlider();
        }
    }

    void FillresetSlider()
    {
        if (resetSlider.value < 1f)
        {
            resetSlider.value += fillSpeed * Time.deltaTime;
        }

        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void EmptyresetSlider()
    {
        if (resetSlider.value > 0f)
        {
            resetSlider.value -= fillSpeed * Time.deltaTime;
        }
    }
}
