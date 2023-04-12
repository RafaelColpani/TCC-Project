using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    #region Vars

    #region Inspector
    [Header("Main Menu")]
    [SerializeField] MainMenuManager mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] Button backButton;
    #endregion

    #region Private vars
    private bool isLoadingGame = false;

    private SaveSlot[] saveSlots;
    #endregion

    #endregion

    private void Awake() => saveSlots = this.GetComponentsInChildren<SaveSlot>();

    #region Events
    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        DataPersistanceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        

        if (!isLoadingGame)
        {
            DataPersistanceManager.Instance.NewGame();
            SceneManager.LoadScene("Save_1");
            return;
        }

        SceneManager.LoadScene(DataPersistanceManager.Instance.GetActiveScene());
    }
    #endregion

    #region Methods
    public void ActivateMenu(bool isLoadingGame = false)
    {
        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        Dictionary<string, SaveData> profilesSaveData = DataPersistanceManager.Instance.GetAllProfilesSaveData();

        foreach (SaveSlot slot in saveSlots)
        {
            SaveData profileData = null;
            profilesSaveData.TryGetValue(slot.GetProfileId(), out profileData);
            slot.SetData(profileData);

            if (profileData == null && isLoadingGame)
                slot.SetInteractable();

            else
                slot.SetInteractable(true);
        }
    }

    public void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
            saveSlot.SetInteractable();

        backButton.interactable = false;
    }

    public void DeactivateMenu() => this.gameObject.SetActive(false);
    #endregion
}
