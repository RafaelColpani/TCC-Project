using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] SaveSlotsMenu saveSlotsMenu;

    [Header("Buttons")]
    [SerializeField] Button newGameButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button loadButton;

    private void Start()
    {
        if (!DataPersistanceManager.Instance.HasGameData())
        {
            continueButton.interactable = false;
            loadButton.interactable = false;
        }
    }

    public void OnNewGame()
    {
        saveSlotsMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGame()
    {
        DisableMenuButtons();
        SceneManager.LoadSceneAsync(DataPersistanceManager.Instance.GetActiveScene());
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }

    public void ActivateMenu() => this.gameObject.SetActive(true);

    public void DeactivateMenu() => this.gameObject.SetActive(false);
}
