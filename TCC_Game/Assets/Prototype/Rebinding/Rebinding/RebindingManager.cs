// Created by: Henrique Batista de Assis
// Date: 27/12/2022

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject buttonsContainer;
    [SerializeField] GameObject waitingInputMenu;

    [Header("UI")]
    [SerializeField] TMP_Text waitingInputTxt;

    [Header("Acions")]
    [SerializeField] InputActionAsset actions;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation; // to store a rebinding operation process
    private RebindingButton[] rebindingButtons;

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

        // UI action map disable here
        waitingInputMenu.SetActive(true);

        // needed to store the operation in an instance to avoid memory leak
        rebindingOperation = rebindingButton.rebindAction.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse") // to exclude some control input
            .OnMatchWaitForAnother(0.1f) // a delay after rebinding, before finish the action. Reccomended by the documentation
            .OnComplete(operation => RebindingCompleted(rebindingButton, action, bindingIndex)) // code to foloow when rebinding is completed
            .OnCancel(operation => RebindingCanceled()) // code to follow when rebinding is canceled
            .Start(); // to start the operation
    }

    private void RebindingCompleted(RebindingButton rb, InputAction action, int bindingIndex)
    {
        rb.buttonText.text = InputControlPath.ToHumanReadableString(
            action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose(); // to finish the operation, avoiding memory leak, MUST HAVE
        waitingInputMenu.SetActive(false);
        SaveBindings();
    }

    private void RebindingCanceled()
    {
        rebindingOperation.Dispose(); // to finish the operation, avoiding memory leak
        waitingInputMenu.SetActive(false);
    }

    public void ResetBindings()
    {
        foreach (RebindingButton rb in rebindingButtons)
        {
            if (!HaveBindingSelected(rb, out var action, out var bindingIndex)) return;

            action.RemoveBindingOverride(bindingIndex);

            rb.buttonText.text = InputControlPath.ToHumanReadableString(
                action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }

        SaveBindings();
    }

    public void SaveBindings()
    {
        string rebinds = actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void LoadBindings()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            actions.LoadBindingOverridesFromJson(rebinds);
        }
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