using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public AudioSource click, hover, close;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("nome do obj clicado: " + eventData.selectedObject.name);
        click.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        close.Play();
    }
}
