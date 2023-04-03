using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Implementa o item com as funções de Drag.
    public Item item;
    [HideInInspector] public Image itemUI;

    [HideInInspector] public int count = 1; // conta o quanto de items estão empilhados
    /*[HideInInspector]*/ public TextMeshProUGUI countText;
    public Image countImageBase;

    public TextMeshProUGUI itemName;
    CanvasGroup canvasGroup;

    [HideInInspector]
    public Transform initialParentSlot; // Se o jogador soltar no OnDrag, ele volta para a posição inicial


    private void Start() {
        itemUI = GetComponent<Image>();
        canvasGroup = itemName.GetComponent<CanvasGroup>();
    }

    IEnumerator ShowItemName(InventoryItem item)
    {
        //chatgpt helped
        //float delay = .5f;
        float fadeTime = 1f;
        //CanvasGroup canvasGroup = item.gameObject.GetComponentInChildren<CanvasGroup>();
        
        if(canvasGroup == null)
            canvasGroup = itemName.GetComponent<CanvasGroup>();

        canvasGroup.gameObject.SetActive(true);

        float alpha = 1f;
        //yield return new WaitForSecondsRealtime(delay);
        while (alpha > 0f)
        {
            //if (alpha == 1f)
            //    yield return new WaitForSecondsRealtime(delay);
            alpha -= Time.deltaTime / fadeTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.gameObject.SetActive(false);
    }

    public void InitializeItem(Item newItem)
    {

        if    (itemUI == null)      itemUI = GetComponent<Image>();
        if (countText == null)   countText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (itemName == null)     itemName = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        /* GetComponentInChildren<TextMeshProUGUI>();*/

        //itemName.gameObject.SetActive(false);

        item = newItem;
        itemUI.sprite = item.uiSprite;
        itemName.text = item.name;
        StopAllCoroutines();
        StartCoroutine("ShowItemName", this);

        RefreshCount();
    }

    

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1; // se count>1 for true
        countText.gameObject.SetActive(textActive);
        countImageBase.gameObject.SetActive(textActive);

        StopAllCoroutines();
        StartCoroutine("ShowItemName", this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialParentSlot = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemUI.raycastTarget = false;

        StopAllCoroutines();
        StartCoroutine("ShowItemName", this);
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
    
