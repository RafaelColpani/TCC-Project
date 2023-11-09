// Created by: Henrique Batista de Assis
// Date: 27/12/2022

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingButton : MonoBehaviour
{
    #region Inspector
    [Header("Input System")]
    [Tooltip("The reference of the specified action in the action map.")]
    public InputActionReference rebindAction;
    [Tooltip("The id of the binding in string form")]
    public string bindingId;
    [Tooltip("The format that will be displayed the Binding ID in inspector")]
    [SerializeField] InputBinding.DisplayStringOptions displayStringOptions;

    [Header("UI")]
    public TMP_Text buttonText;
    #endregion

    #region Non-Inpector
    [HideInInspector] public int Id { get; private set; } // ID will be also the button index in the RebindingManager array
    [HideInInspector] public RebindingManager Rm { get; set; }
    #endregion

    #region Unity Methods
    private void Start()
    {
        if (!Rm.HaveBindingSelected(this, out var action, out var bindingIndex))
        {
            Debug.LogError($"Forgetted to select a binding in the inspector of the button game object {this.gameObject.name}");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            Debug.LogError("This game does not support rebinding composite key, change this in the inspector of the rebinding button");
            return;
        }

        Rm.LoadBindings();

        buttonText.text = BindingUtils.GetButtonImageTextByInputAction(action, bindingIndex);
    }
    #endregion

    #region Methods
    public void SetId(int value)
    {
        this.Id = value;
    }

    public void OnClickedRebind()
    {
        Rm.StartRebindingKey(this);
    }
    #endregion
}
