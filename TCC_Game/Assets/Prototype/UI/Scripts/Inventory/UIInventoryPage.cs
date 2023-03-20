using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private GameObject slotsGroup;
    [SerializeField] List<UIInventoryItem> listOfItens = new List<UIInventoryItem>();

    private void Start()
    {
        if (slotsGroup == null)
            GameObject.Find("grp_inventorySlots");

        InitializeInventoryUI(10);
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i=0; i < inventorySize; i++)
        {
            InstantiateItem(itemPrefab, slotsGroup.transform.GetChild(i), i);
        }
    }

    public void InstantiateItem(UIInventoryItem item, Transform t, int i)
    {
        UIInventoryItem _item = Instantiate(item, t);
        _item.name = "slotItem_" + i;
        AddItemUI(_item);
    }

    public void AddItemUI(UIInventoryItem item)
    {
        listOfItens.Add(item);
    }
}
