using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject InventoryItemPrefab;
    public int maxStackedItems = 4;

    int selectedSlot = -1;

    float scrollSpeed = 1f;
    
    private void Start()
    {
        // first slot selected by default
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        // adaptar para input system se necessario
        if(Input.inputString != null)
        {
            // verifica se é um número, e se for, o extrai e usa como index
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 4)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot]; //temp
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if(use == true)
            {
                itemInSlot.count--;

                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                    itemInSlot.RefreshCount();
            }

            return item;
        }
        return null;
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
            inventorySlots[selectedSlot].Deselect();

        if (newValue > 3)
            newValue = 3;

        if (newValue < 0)
            newValue = 0;

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    

    // Verifica se ha algum slot com o mesmo item com a quantiadade menor que o maximo
    public bool AddItem(Item item)
    {
        // STACKING
        for (int i = 0; i < inventorySlots.Length; i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if(
             itemInSlot != null &&
             itemInSlot.item == item &&
             itemInSlot.count < maxStackedItems &&
             itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // SLOT VAZIO
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot); // Pode adicionar o item
                return true;
            }
        }

        // TODOS JÁ ESTÃO CHEIOS
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
        
    }
}
