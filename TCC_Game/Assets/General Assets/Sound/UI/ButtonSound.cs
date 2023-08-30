using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler //, IPointerUpHandler
{
    public UISounds soundManager;
    public AudioSource click, hover, close;

    public bool isExitButton = false;
    private void Start()
    {
        if(!soundManager)
            soundManager = FindObjectOfType<UISounds>();

        click = soundManager.click;
        hover = soundManager.hover;
        close = soundManager.close;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        print("pointer clicked" + name);
        if (!isExitButton)
            click.Play();
        else
            close.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("pointer entered" + name);
        hover.Play();
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}
}
