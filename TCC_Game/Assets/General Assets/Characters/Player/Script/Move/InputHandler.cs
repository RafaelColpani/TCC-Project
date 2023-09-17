using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using KevinCastejon.MoreAttributes;
using static UnityEditor.MaterialProperty;
using static UnityEngine.Rendering.DebugUI;
using System.Numerics;
using UnityEngine.TextCore.Text;

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
    private PlayerShooter playerShooter;
    private PlayerInteract playerInteract;
    private InventoryManager inventoryManager;

    private Transform body;
    private Transform groundCheck;

    private LayerMask groundLayer;

    private float groundCheckRadius;

    [HideInInspector] public bool canWalk = true;

    #region Commands
    private MoveCommand moveCommand;
    private PressJumpCommand pressJumpCommand;
    private ReleaseJumpCommand releaseJumpCommand;
    private ShootCommand shootCommand;
    private InteractionCommand interactionCommand;
    private SkipDialogueCommand skipDialogueCommand;
    private DropItemCommand dropItemCommand;
    #endregion

    #endregion

    #region Unity Methods
    private void Awake()
    {
        InitializeInstantiations();
        LoadInputBindings();
        InitializeCommands();
        AssignCommands();
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;
        if (!canWalk) return;

        var readedMoveValue = playerActions.Movement.Move.ReadValue<float>();
        moveCommand.Execute(this.gameObject.transform, readedMoveValue, characterController);
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

    private void InitializeInstantiations()
    {
        playerActions = new PlayerActions();

        characterController = GetComponent<CharacterController>();
        gravityController = GetComponent<GravityController>();
        characterManager = GetComponent<CharacterManager>();
        proceduralLegs = GetComponent<ProceduralLegs>();
        playerShooter = GetComponentInChildren<PlayerShooter>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        inventoryManager = GameObject.Find("_InventoryManager").GetComponent<InventoryManager>();

        this.body = characterManager.Body;
        this.groundCheck = characterManager.GroundCheckParent;
        this.groundLayer = characterManager.GroundLayers;
        this.groundCheckRadius = characterManager.GroundCheckDistance;
    }

    private void InitializeCommands()
    {
        moveCommand = new MoveCommand(proceduralLegs, walkSpeed);
        pressJumpCommand = new PressJumpCommand(jumpForce, groundCheck, groundCheckRadius, groundLayer, gravityController);
        releaseJumpCommand = new ReleaseJumpCommand();
        shootCommand = new ShootCommand(playerShooter);
        interactionCommand = new InteractionCommand(playerInteract);
        skipDialogueCommand = new SkipDialogueCommand();
        dropItemCommand = new DropItemCommand(inventoryManager);
    }

    private void AssignCommands()
    {
        playerActions.Movement.Jump.performed += ctx => pressJumpCommand.Execute(body);
        playerActions.Movement.Jump.canceled += ctx => releaseJumpCommand.Execute(body);

        playerActions.Movement.Shoot.performed += ctx => shootCommand.Execute(body);
        playerActions.Movement.Interaction.performed += ctx => interactionCommand.Execute(body);

        playerActions.Movement.SkipDialogue.performed += ctx => skipDialogueCommand.Execute(body);

        playerActions.Movement.DropItem.performed += ctx => dropItemCommand.Execute(body);
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