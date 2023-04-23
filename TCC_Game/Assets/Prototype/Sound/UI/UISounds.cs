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

    }
}
