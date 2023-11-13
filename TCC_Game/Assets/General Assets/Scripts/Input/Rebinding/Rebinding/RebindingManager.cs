// Created by: Henrique Batista de Assis
// Date: 27/12/2022

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using KevinCastejon.MoreAttributes;
using UnityEngine.EventSystems;

public class RebindingManager : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- OBJECTS -", (int)HeaderPlusColor.green)]
    [Tooltip("The parent object that contains all buttons with the RebindingButton script attached.")]
    [SerializeField] GameObject buttonsContainer;
    [Tooltip("The object that will pop up when it is waiting for the user to press an input.")]
    [SerializeField] GameObject waitingInputMenu;

    [HeaderPlus(" ", "- ACTIONS -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The input actions that the rebinding will work with. MUST BE PlayerActions.")]
    [SerializeField] InputActionAsset actions;
    #endregion

    #region Private Vars
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation; // to store a rebinding operation process
    private RebindingButton[] rebindingButtons;
    private readonly string rebindsKey = "playerRebinds";

    private GameObject selectedBindingButton;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rebindingButtons = buttonsContainer.GetComponentsInChildren<RebindingButton>();
        foreach(RebindingButton rb in rebindingButtons)
        {
            rb.Rm = this;
        }
    }
    #endregion

    #region Methods
    public void StartRebindingKey(RebindingButton rebindingButton)
    {
        if (!HaveBindingSelected(rebindingButton, out var action, out var bindingIndex))
            return;

        if (action.bindings[bindingIndex].isComposite)
        {
            Debug.LogError("This game does not support rebinding composite key, change this in the inspector of the rebinding button");
            return;
        }

        selectedBindingButton = rebindingButton.gameObject;
        EventSystem.current.SetSelectedGameObject(null);

        // UI action map disable here
        if (waitingInputMenu != null)
            waitingInputMenu.SetActive(true);

        // needed to store the operation in an instance to avoid memory leak
        rebindingOperation = rebindingButton.rebindAction.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse") // to exclude some control input
            .WithCancelingThrough("<Keyboard>/escape") // calls the cancel with escape
            .OnMatchWaitForAnother(0.1f) // a delay after rebinding, before finish the action. Reccomended by the documentation
            .OnComplete(operation => RebindingCompleted(rebindingButton, action, bindingIndex)) // code to folow when rebinding is completed
            .OnCancel(operation => RebindingCanceled()) // code to follow when rebinding is canceled
            .Start(); // to start the operation
    }

    private void RebindingCompleted(RebindingButton rb, InputAction action, int bindingIndex)
    {
        rb.buttonText.text = BindingUtils.GetButtonImageTextByInputAction(action, bindingIndex);

        rebindingOperation.Dispose(); // to finish the operation, avoiding memory leak, MUST HAVE

        if (waitingInputMenu != null)
            waitingInputMenu.SetActive(false);

        if (selectedBindingButton != null)
        {
            EventSystem.current.SetSelectedGameObject(selectedBindingButton);
            selectedBindingButton = null;
        }

        SaveBindings();
    }

    private void RebindingCanceled()
    {
        rebindingOperation.Dispose(); // to finish the operation, avoiding memory leak

        if (selectedBindingButton != null)
        {
            EventSystem.current.SetSelectedGameObject(selectedBindingButton);
            selectedBindingButton = null;
        }

        if (waitingInputMenu != null)
            waitingInputMenu.SetActive(false);
    }

    public void ResetBindings()
    {
        foreach (RebindingButton rb in rebindingButtons)
        {
            if (!HaveBindingSelected(rb, out var action, out var bindingIndex)) return;

            action.RemoveBindingOverride(bindingIndex);

            rb.buttonText.text = BindingUtils.GetButtonImageTextByInputAction(action, bindingIndex);
        }

        SaveBindings();
    }

    public void SaveBindings()
    {
        string rebinds = actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(rebindsKey, rebinds);
        SaveInputHandler();
    }

    public void LoadBindings()
    {
        string rebinds = PlayerPrefs.GetString(rebindsKey, string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            actions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void SaveInputHandler()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInChildren<InputHandler>().LoadInputBindings();
    }

    /// <summary>
    /// To resolve and get the binding index selected from the inspector
    /// </summary>
    /// <param name="action"></param>
    /// <param name="bindingIndex"></param>
    /// <returns></returns>
    public bool HaveBindingSelected(RebindingButton rb, out InputAction action, out int bindingIndex)
    {
        bindingIndex = -1;
        action = rb.rebindAction?.action;

        if (action == null) return false;
        if (string.IsNullOrEmpty(rb.bindingId)) return false;

        // look for the binding id, by the index of the dropdown in inspector
        var bindingId = new Guid(rb.bindingId);
        bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);

        if (bindingIndex == -1)
        {
            Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
            return false;
        }

        return true;
    }
    #endregion
}