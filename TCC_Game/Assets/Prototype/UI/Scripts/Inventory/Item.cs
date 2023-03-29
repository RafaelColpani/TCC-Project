using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{


    [Header("Gameplay Only")]
    public ItemType type;
    public ActionType actionType;
    
    [Header("UI")]
    public bool stackable = false; // "pilh�vel"
    

    [Header("Both")]
    public Sprite sprite;
    public Image uiSprite;

    public bool edible; // comest�vel

    public enum ItemType
    {
        Food,
        Ammo
    }

    public enum ActionType
    {
        Use,
        Drop
    }
    
}
