using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using KevinCastejon.MoreAttributes;

[RequireComponent(typeof(CharacterController))]
public class InputHandler : MonoBehaviour
{
    #region VARs
    private PlayerActions playerActions;
    private CharacterController characterController;

    #region Inspector VARs
    [HeaderPlus(" ", "- MOVE COMMAND -", (int)HeaderPlusColor.green)]
    [Tooltip("Controls the speed of player walk.")]
    [SerializeField] private float walkSpeed;

    [HeaderPlus(" ", "- JUMP COMMAND -", (int)HeaderPlusColor.yellow)]
    [Tooltip("Controls the force (aka height) of player jump.")]
    [SerializeField] private float jumpForce;
    #endregion

    #region Commands
    private MoveCommand moveCommand;
    private PressJumpCommand pressJumpCommand;
    private ReleaseJumpCommand releaseJumpCommand;
    #endregion
    #endregion

    #region Unity Methods
    private void Awake() 
    {
        playerActions = new PlayerActions();
        characterController = GetComponent<CharacterController>();

        LoadInputBindings();
        InitializeCommands();
        AssignCommands();
    }

    private void Update() 
    {
        var readedMoveValue = playerActions.Movement.Move.ReadValue<float>();
        moveCommand.Execute(this.gameObject, characterController, readedMoveValue);
    }
    #endregion

    #region Internal Methods
    /// <summary>Loads the binding keys personalizated by the player or not.</summary>
    public void LoadInputBindings()
    {
        /* REBINDING --
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
            playerActions.LoadBindingOverridesFromJson(rebinds);
        */
    }

    private void InitializeCommands()
    {
        moveCommand = new MoveCommand(walkSpeed);
        pressJumpCommand = new PressJumpCommand(jumpForce);
        releaseJumpCommand = new ReleaseJumpCommand();
    }

    private void AssignCommands()
    {
        playerActions.Movement.Jump.performed += ctx => pressJumpCommand.Execute(this.gameObject);
        playerActions.Movement.Jump.canceled += ctx => releaseJumpCommand.Execute(this.gameObject);
    }
    #endregion

    #region Enable & Disable
    private void OnEnable() 
    {
        playerActions.Enable();
    }

    private void OnDisable() 
    {
        playerActions.Disable();
    }
    #endregion
}