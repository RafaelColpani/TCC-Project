using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    public void PickupItem(int id)
    {
        bool hasFreeSlot = inventoryManager.AddItem(itemsToPickup[id]);

        // se ha um slot livre
        if (hasFreeSlot == true)
            print("ADDED");

        else
            print("NOT ADDED: NO SPACE LEFT");
    }

    public void RemoveAllItems()
    {
        inventoryManager.RemoveAllItems();
    }

    // chamado com X condicao
    public void RemoveItem()
    {
        // obtem slot do item selecionado
        inventoryManager.GetSelectedItem(false);
        // Destroi gObject do item
    }

    public void UseSelectedItem()
    {
        inventoryManager.GetSelectedItem(true);
    }

    public void GetSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if (receivedItem != null)
        {
            print("item received");
        }
        else
            print("no item received");
    }

    // estou ficando bilu teteia por isso parei de pensar e vou commitar, obrigado
    public void UseSelectedItem2()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(true);
        if (receivedItem != null)
        {
            print("item used");
        }
        else
            print("no item used");
    }
}