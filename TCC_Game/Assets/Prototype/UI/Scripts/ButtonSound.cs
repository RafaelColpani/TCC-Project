using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public UISounds soundManager;
    public AudioSource click, hover, close;

    public bool isExitButton = false;
    private void Start()
    {

        click = soundManager.click;
        hover = soundManager.hover;
        close = soundManager.close;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isExitButton)
            click.Play();
        else
            close.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.Play();
    }

}
