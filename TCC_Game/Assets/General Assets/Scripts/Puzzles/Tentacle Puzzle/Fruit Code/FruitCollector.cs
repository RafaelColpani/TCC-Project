using System;
using UnityEngine;

public class FruitCollector : MonoBehaviour, IInteractable
{
    [SerializeField] Transform uiPopUp;

    #region Private Vars
    private readonly string popUpTag = "UIPopUp";
    private ProceduralArms playerArms;
    private bool isInteractable = true;
    #endregion

    #region Unity Methods
    private void Start()
    {
        playerArms = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ProceduralArms>();
    }

    private void Update()
    {
        if (PauseController.GetIsPaused()) return;
        if (uiPopUp == null || !uiPopUp.gameObject.activeSelf) return;

        
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

        if (uiPopUp != null)
            uiPopUp.gameObject.SetActive(false);

        playerArms.CarryObject(this.gameObject);
    }
    #endregion

    #region Collision Events
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (uiPopUp == null) return;

        if ((playerArms.IsCarryingObject && uiPopUp.gameObject.activeSelf) || (uiPopUp.gameObject.activeSelf && !isInteractable))
            uiPopUp.gameObject.SetActive(false);

        if (!isInteractable) return;
        if (playerArms.IsCarryingObject) return;

        uiPopUp.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (uiPopUp == null) return;
        if (playerArms.IsCarryingObject) return;

        uiPopUp.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        isInteractable = true;
    }
    #endregion
}
