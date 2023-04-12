using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using KevinCastejon.MoreAttributes;

[RequireComponent(typeof(GravityController))]
public class InputHandler : MonoBehaviour
{
    #region VARs
    private PlayerActions playerActions;
    private CharacterController characterController;
    private GravityController gc;

    #region Inspector VARs
    [HeaderPlus(" ", "- MOVE COMMAND -", (int)HeaderPlusColor.green)]
    [Tooltip("Controls the speed of player walk.")]
    [SerializeField] private float walkSpeed;

    [HeaderPlus(" ", "- JUMP COMMAND -", (int)HeaderPlusColor.yellow)]
    [Tooltip("Controls the force (aka height) of player jump.")]
    [SerializeField] private float jumpForce;
    [Tooltip("The transform in the position that will check if the object is grounded.")]
    [SerializeField] private Transform groundCheck;
    [Tooltip("The radius of the circle that will detect the ground from the checkGround Transform position.")]
    [SerializeField] private float groundCheckRadius;
    [Tooltip("The layer(s) that will be considered ground to perform a jump.")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject body;
    #endregion

    #region Commands
    private MoveCommand moveCommand;
    private PressJumpCommand pressJumpCommand;
    private ReleaseJumpCommand releaseJumpCommand;
    #endregion

    #region VARs
    public bool jumped = false;
    #endregion

    #endregion

    #region Unity Methods
    private void Awake() 
    {
        playerActions = new PlayerActions();
        characterController = GetComponent<CharacterController>();
        gc = GetComponent<GravityController>();

        LoadInputBindings();
        InitializeCommands();
        AssignCommands();
    }

    private void FixedUpdate() 
    {
        var readedMoveValue = playerActions.Movement.Move.ReadValue<float>();
        moveCommand.Execute(this.gameObject, characterController, readedMoveValue);

        if (jumped && JumpUtils.IsGrounded(groundCheck, groundCheckRadius, groundLayer))
        {
            pressJumpCommand.Execute(this.gameObject, characterController);
        }
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
        pressJumpCommand = new PressJumpCommand(jumpForce, groundCheck, groundCheckRadius, groundLayer, gc);
        releaseJumpCommand = new ReleaseJumpCommand();
    }

    private void AssignCommands()
    {
        playerActions.Movement.Jump.performed += ctx => pressJumpCommand.Execute(body);
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