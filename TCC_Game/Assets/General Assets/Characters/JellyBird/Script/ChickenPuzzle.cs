using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class ChickenPuzzle : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- CHICKEN -", (int)HeaderPlusColor.green)]
    [Tooltip("the script located in the JellyBird")]
    [SerializeField] ChickenFruitFollow chickenFruitFollow;

    [HeaderPlus(" ", "- FRUIT -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The tag of the fruit game objects")]
    [SerializeField] string fruitTag;
    #endregion

    #region Unity Events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(fruitTag)) return;

        chickenFruitFollow.TriggerFruitFollow(collision.gameObject);
    }
    #endregion
}