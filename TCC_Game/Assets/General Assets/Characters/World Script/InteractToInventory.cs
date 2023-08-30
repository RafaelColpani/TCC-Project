using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractToInventory : MonoBehaviour, IInteractable
{
    InventoryManager inventoryM;
    [SerializeField] Item item;

    [Header("playtest")]
    [SerializeField] GameObject aviso;

    public void Interact()
    {
        print("interagindo com "+this.gameObject.name);
        switch (item.name)
        {
            case "item_artifactSummer":
                DialogueConditions.hasSummer = true;
                print(DialogueConditions.hasSummer);
                break;
            case "item_artifactAutumn":
                DialogueConditions.hasAutumn = true;
                print(DialogueConditions.hasAutumn);
                break;
            case "item_artifactWinter":
                DialogueConditions.hasWinter = true;
                print(DialogueConditions.hasWinter);
                break;
            default:
                break;
        }

        //playtestLeveParaNPC();
        
        inventoryM = GameObject.Find("_InventoryManager").GetComponent<InventoryManager>();
        inventoryM.AddItem(item);
        
        Destroy(this.gameObject);
    }
    
    /*
    void playtestLeveParaNPC(){
        aviso = GameObject.Find("Canvas_UI").transform.Find("grp_leveParaNpc").gameObject;
        print(aviso.name);
        aviso.SetActive(true);
        aviso.GetComponent<disableAfterSeconds>().startDisabling(5);
    }
    */
}
