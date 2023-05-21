using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using KevinCastejon.MoreAttributes;

[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterManager))]
[RequireComponent(typeof(ProceduralLegs))]
public class InputHandler : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- MOVE COMMAND -", (int)HeaderPlusColor.yellow)]
    [Tooltip("Controls the speed of player walk.")]
    [SerializeField] private float walkSpeed;

    [HeaderPlus(" ", "- JUMP COMMAND -", (int)HeaderPlusColor.cyan)]
    [Tooltip("Controls the force (aka height) of player jump.")]
    [SerializeField] private float jumpForce;
    #endregion

    #region VARs
    private PlayerActions playerActions;
    private CharacterController characterController;
    private GravityController gravityController;
    private CharacterManager characterManager;
    private ProceduralLegs proceduralLegs;

    private Transform body;
    private Transform groundCheck;

    private LayerMask groundLayer;

    private float groundCheckRadius;

    [HideInInspector] public bool canWalk = true;

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
        characterManager = GetComponent<CharacterManager>();
        proceduralLegs = GetComponent<ProceduralLegs>();

        this.body = characterManager.Body;
        this.groundCheck = characterManager.GroundCheckParent;
        this.groundLayer = characterManager.GroundLayers;
        this.groundCheckRadius = characterManager.GroundCheckDistance;

        LoadInputBindings();
        InitializeCommands();
        AssignCommands();
    }

    private void FixedUpdate() 
    {
        if (PauseController.GetIsPaused()) return;
        if (!canWalk) return;
        var readedMoveValue = playerActions.Movement.Move.ReadValue<float>();
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
        moveCommand = new MoveCommand(proceduralLegs, walkSpeed);
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
    }

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
