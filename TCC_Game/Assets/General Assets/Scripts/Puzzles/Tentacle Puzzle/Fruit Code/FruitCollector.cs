using System;
using UnityEngine;

public class FruitCollector : MonoBehaviour, IInteractable
{
    #region Private Vars
    private readonly string popUpTag = "UIPopUp";
    private GameObject uiPopUp;
    private ProceduralArms playerArms;
    private bool isInteractable = true;
    #endregion

    #region Unity Methods
    private void Start()
    {
        playerArms = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ProceduralArms>();

        foreach (Transform child in this.transform)
        {
            if (!child.gameObject.CompareTag(popUpTag)) continue;

            uiPopUp = child.gameObject;
            break;
        }
    }
    #endregion

    #region Public Methods
    public void DisableIsInteractable()
    {
        isInteractable = false;
    }
    #endregion

    #region Interface
    void IInteractable.Interact()
    {
        if (!isInteractable) return;
        if (playerArms.IsCarryingObject) return;

        playerArms.CarryObject(this.gameObject);
    }
    #endregion

    #region Collision Events
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (uiPopUp == null) return;
        if (playerArms.IsCarryingObject) return;

        uiPopUp.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (uiPopUp == null) return;
        if (playerArms.IsCarryingObject) return;

        uiPopUp.SetActive(false);
    }

    private void OnEnable()
    {
        isInteractable = true;
    }
    #endregion
}
