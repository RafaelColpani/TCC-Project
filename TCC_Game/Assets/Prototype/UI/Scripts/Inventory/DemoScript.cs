using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    public void PickupItem(int id)
    {
        bool freeSlot = inventoryManager.AddItem(itemsToPickup[id]);

        if (freeSlot == true)
            print("ADDED");

        else 
            print("NOT ADDED: NO SPACE LEFT");
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

    public void UseSelectedItem()
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
