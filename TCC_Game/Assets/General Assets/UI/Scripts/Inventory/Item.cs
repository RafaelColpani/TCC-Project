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
    public GameObject PrefabReference;
    public int bellyFiller;

    [Header("UI")]
    public bool stackable = false; // "pilh�vel"
    

    [Header("Both")]
    public Sprite sprite;
    public Sprite uiSprite;

    public bool edible; // comest�vel
    public bool isArtifact = false; // variavel provavelmente inutil, mas veremos

    public enum ItemType
    {
        Food,
        Ammo,
        Artifact
    }

    public enum ActionType
    {
        Use,
        Drop,
        None
    }
    
}   
