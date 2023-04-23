using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractToInventory : MonoBehaviour, IInteractable
{
    InventoryManager inventoryM;
    [SerializeField] Item item;

    public void Interact()
    {
        switch (item.name)
        {
            case "item_artifactSummer":
                DialogueConditions.hasSummer = true;
                break;
            case "item_artifactAutumn":
                DialogueConditions.hasAutumn = true;
                break;
            case "item_artifactWinter":
                DialogueConditions.hasWinter = true;
                break;
            default:
                break;
        }
        inventoryM = GameObject.Find("_InventoryManager").GetComponent<InventoryManager>();
        inventoryM.AddItem(item);
        Destroy(this.gameObject);
    }

}
