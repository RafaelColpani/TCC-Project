using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Implementa o item com as funções de Drag.
    [HideInInspector] public Item item;
    [HideInInspector] public Image itemUI;

    [HideInInspector]
    public Transform initialParentSlot; // Se o jogador soltar no OnDrag, ele volta para a posição inicial

    private void Start()
    {
        itemUI = GetComponent<Image>();
    }

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        itemUI.sprite = item.uiSprite;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        initialParentSlot = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemUI.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        #region MOUSE_INPUT

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 1;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);        

        #endregion
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(initialParentSlot);
        itemUI.raycastTarget = true;
    }
}
    
