using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using KevinCastejon.MoreAttributes;

[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ProceduralLegs))]
public class InputHandler : MonoBehaviour
{
    #region VARs
    private PlayerActions playerActions;
    private CharacterController characterController;
    private GravityController gravityController;
    private ProceduralLegs proceduralLegs;

    public bool canWalk = true;
    #region Inspector VARs
    [HeaderPlus(" ", "- GENERAL -", (int)HeaderPlusColor.green)]
    [Tooltip("The Transform of the player's body.")]
    [SerializeField] private Transform body;

    [HeaderPlus(" ", "- MOVE COMMAND -", (int)HeaderPlusColor.yellow)]
    [Tooltip("Controls the speed of player walk.")]
    [SerializeField] private float walkSpeed;

    [HeaderPlus(" ", "- JUMP COMMAND -", (int)HeaderPlusColor.cyan)]
    [Tooltip("Controls the force (aka height) of player jump.")]
    [SerializeField] private float jumpForce;
    [Tooltip("The transform in the position that will check if the object is grounded.")]
    [SerializeField] private Transform groundCheck;
    [Tooltip("The radius of the circle that will detect the ground from the checkGround Transform position.")]
    [SerializeField] private float groundCheckRadius;
    [Tooltip("The layer(s) that will be considered ground to perform a jump.")]
    [SerializeField] private LayerMask groundLayer;
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
        gravityController = GetComponent<GravityController>();
        proceduralLegs = GetComponent<ProceduralLegs>();

        LoadInputBindings();
        InitializeCommands();
        AssignCommands();
    }

    private void FixedUpdate() 
    {
        if (!canWalk) return;
        var readedMoveValue = playerActions.Movement.Move.ReadValue<float>();
        moveCommand.ChangeMoveDirection(proceduralLegs.GetMeanLegsDirection());
        moveCommand.Execute(this.gameObject.transform, characterController, readedMoveValue);
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
        pressJumpCommand = new PressJumpCommand(jumpForce, groundCheck, groundCheckRadius, groundLayer, gravityController);
        releaseJumpCommand = new ReleaseJumpCommand();
    }

    private void AssignCommands()
    {
        playerActions.Movement.Jump.performed += ctx => pressJumpCommand.Execute(body);
        playerActions.Movement.Jump.canceled += ctx => releaseJumpCommand.Execute(body);
    }

    public void SetCanWalk(bool value = false)
    {
        if (value)
        {
            StartCoroutine(DelayedCanWalk());
        }

        else
        {
            canWalk = value;
        }

        moveCommand.SetCanWalk(value);
    }

    IEnumerator DelayedCanWalk()
    {
        yield return new WaitForSeconds(1);
        canWalk = true;

    public MoveCommand GetMovementCommand()
    {
        return this.moveCommand;
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
