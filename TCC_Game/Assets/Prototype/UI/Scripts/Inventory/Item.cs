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
    public bool stackable = false; // "pilhável"
    

    [Header("Both")]
    public Sprite sprite;
    public Image uiSprite;

    public bool edible; // comestível

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
