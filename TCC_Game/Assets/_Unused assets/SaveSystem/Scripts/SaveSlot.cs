using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    #region Vars

    #region Inspector
    [Header("Profile")]
    [Tooltip("The profile ID of the save data that this save slot represents.")]
    [SerializeField] string profileId;

    [Header("Content")]
    [SerializeField] GameObject noDataContent;
    [SerializeField] GameObject hasDataContent;
    #endregion

    private Button saveSlotButton;
    #endregion

    private void Awake() => saveSlotButton = GetComponent<Button>();

    /// <summary>
    /// Shows the slot button accordingly if has a data or not. 
    /// </summary>
    /// <param name="data"></param>
    public void SetData(SaveData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }

        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
        }
    }

    /// <summary>
    /// Sets if the button of save Slot is interactable (true) or not (false). Bool parameter is false by default.
    /// </summary>
    /// <param name="isInteractable"></param>
    public void SetInteractable(bool isInteractable = false) => this.saveSlotButton.interactable = isInteractable;

    public string GetProfileId()
    {
        return this.profileId;
    }
}
