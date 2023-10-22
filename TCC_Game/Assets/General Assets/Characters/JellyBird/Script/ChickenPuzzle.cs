using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class ChickenPuzzle : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- EVENTS -", (int)HeaderPlusColor.green)]
    [Tooltip("the script located in the JellyBird")]
    [SerializeField] ChickenFruitFollow chickenFruitFollow;
    [Tooltip("The script that controls the waterfall event")]
    [SerializeField] DestroyEvent destroyEvent;

    [HeaderPlus(" ", "- FRUIT -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The tag of the fruit game objects")]
    [SerializeField] string fruitTag;
    #endregion

    #region Unity Events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(fruitTag)) return;
        if (destroyEvent.IsActive) return;

        chickenFruitFollow.TriggerFruitFollow(collision.gameObject);
    }
    #endregion
}
