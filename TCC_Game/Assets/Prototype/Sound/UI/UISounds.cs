using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISounds : MonoBehaviour
{
    public AudioSource click, hover, close;

    Button[] buttons;

    public GameObject[] closeButtons;

    private void OnEnable()
    {
        buttons = FindObjectsOfType<Button>();
    }

    private void Start()
    {
        //if (buttons != null)
        //    print("is not null");
        //else
        //    print("is null");

        //foreach (Button button in buttons)
        //{
        //    for (int i = 0; i < closeButtons.Length; i++)
        //    {
        //        if(button == closeButtons[i])
        //        {
        //            button.gameObject.AddComponent<ButtonSound>();
        //            button.gameObject.AddComponent<ButtonSound>().isExitButton = true;
        //        }
        //        else
        //            button.gameObject.AddComponent<ButtonSound>();
        //    }
        //}
    }
}
