using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;

    //public Item[] allItemsList;
    
    public GameObject InventoryItemPrefab;

    public int maxStackedItems = 4;
    
    int selectedSlot = -1;
    
    private void Start()
    {
        // first slot selected by default
        ChangeSelectedSlot(0);
    }

    // controla as teclas (?)
    private void Update()
    {
        // adaptar para input system se necessario
        if(Input.inputString != null)
        {
            InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

            // verifica se é um número, e se for, o extrai e usa como index
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 4)
            {
                ChangeSelectedSlot(number - 1);
            }

            else if (Input.GetKeyDown(KeyCode.E)) // usar
            {
                if (itemInSlot?.item.type == Item.ItemType.Food)
                {
                    print("type: food"); // eat
                }

                else if (itemInSlot?.item.type == Item.ItemType.Ammo)
                {
                    print("type: ammo"); // shot
                }

                else if (itemInSlot?.item.type == Item.ItemType.Artifact)
                {
                    print("type: artifact");
                    return;
                }

                UseSelectedItem();
                print("usou o item");
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // dropar
            {
                if(itemInSlot != null)
                {
                    UseSelectedItem();
                    print("dropou o item");
                }
            }
            else if (Input.GetKeyDown(KeyCode.R)) // remove all items
            {
                RemoveAllItems();
            }

        }
    }


    [Tooltip("Funcao 'atalho' para obter o item selecionado")]
    public Item GetSelectedItem(bool use)
    {
        InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
            return itemInSlot.item;

        return null;
    }

    /// <summary>
    /// Função para usar ou dropar item. Esta função remove o item do inventário e destrói o GameObject.
    /// </summary>
    /// <returns>Item item</returns>
    public Item UseSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;

            // USAR O ITEM
            itemInSlot.count--;

            // Usar item (atribuir evento ou funcao de uso aqui depois)

            if (itemInSlot.count <= 0)
                Destroy(itemInSlot.gameObject);
            else
                itemInSlot.RefreshCount();

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

            if (itemInSlot == null) // se estiver vazio
            {
                // se nao for slot de artefato e o item nao for artefato
                if (slot.isArtifactSlot == false &&
                    item.type != Item.ItemType.Artifact)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
                // se o item for artefato e o slot for pra ele
                else if (slot.isArtifactSlot == true &&
                    item.type == Item.ItemType.Artifact)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }
        }
        // TODOS JÁ ESTÃO CHEIOS
        return false;
    }

    public void RemoveAllItems()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if(inventorySlots[i].GetComponentInChildren<InventoryItem>() != null &&
                !inventorySlots[i].isArtifactSlot)
                Destroy(inventorySlots[i].GetComponentInChildren<InventoryItem>().gameObject);
        }
    }

    /// <summary>
    /// Remove o item do inventário
    /// </summary>
    
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}
