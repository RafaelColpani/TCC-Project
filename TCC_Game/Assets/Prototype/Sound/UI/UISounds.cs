using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISounds : MonoBehaviour
{
    public AudioSource click, hover, close;

    Button[] buttons = FindObjectsOfType<Button>();

    public GameObject[] closeButtons;

    private void Start()
    {
        foreach (Button button in buttons)
        {
            for (int i = 0; i < closeButtons.Length; i++)
            {
                if(button == closeButtons[i])
                {
                    button.gameObject.AddComponent<ButtonSound>();
                    button.gameObject.AddComponent<ButtonSound>().isExitButton = true;
                }
                else
                    button.gameObject.AddComponent<ButtonSound>();
            }
        }
    }
}
