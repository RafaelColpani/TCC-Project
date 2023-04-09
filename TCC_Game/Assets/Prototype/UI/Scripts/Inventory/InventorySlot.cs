using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    // Implementa o slot que ira receber o Drop

    public Image img;
    public Color selectedColor, notSelectedColor;
    public Color artifactSelectedColor, artifactNotSelectedColor;
    public bool isArtifactSlot = false;

    private void Awake()
    {
        img = GetComponent<Image>();
        
        Deselect();
    }

    public void Select()
    {
        img.color = selectedColor;
        if (isArtifactSlot)
            img.color = artifactSelectedColor;
    }

    public void Deselect()
    {
        img.color = notSelectedColor;
        if (isArtifactSlot)
            img.color = artifactNotSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
            SetItemParentSlot(eventData, false);

        else if (transform.childCount <= 1 && transform.name.Contains("artifact"))
            SetItemParentSlot(eventData, true);
    }

    void SetItemParentSlot(PointerEventData eventData, bool isArtefact)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem item = dropped.GetComponent<InventoryItem>();

        // Se o slot for de Artefato, colocar o objeto como filho do primeiro filho (imagem decorativa)
        if (!isArtefact)
            item.initialParentSlot = transform;
        else
            item.initialParentSlot = transform.GetChild(0);
    }
}
