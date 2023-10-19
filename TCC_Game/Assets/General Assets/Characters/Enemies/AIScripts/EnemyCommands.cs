using System.Collections;
using System.Collections.Generic;
using KevinCastejon.MoreAttributes;
using UnityEngine;

[RequireComponent(typeof(ProceduralLegs))]
[RequireComponent(typeof(CharacterManager))]
[RequireComponent(typeof(EnemyAIController))]
[RequireComponent(typeof(ObstacleBlock))]

public class EnemyCommands : MonoBehaviour
{
    #region Inspector
    [HeaderPlus(" ", "- MOVE COMMAND -", (int)HeaderPlusColor.yellow)]
    [Tooltip("Controls the speed of enemy walk when in wandering State. player have an walk speed of 5 atm.")]
    [SerializeField] private float wanderingWalkSpeed;
    [Tooltip("Controls the speed of enemy walk when in chasing State. player have an walk speed of 5 atm.")]
    [SerializeField] private float chasingWalkSpeed;
    #endregion

    #region Commands
    private MoveCommand moveCommand;
    #endregion

    #region Private VARs
    private ProceduralLegs proceduralLegs;
    private CharacterManager characterManager;
    private EnemyAIController enemyAIController;
    private ObstacleBlock obstacleBlock;
    private EnemyCollisionController enemyCollisionController;

    private float walkSpeed;
    private float walkValue;

    private bool canWalk;
    private bool cameToEdge;
    #endregion

    #region Getters & Setters
    public bool CanWalk
    {
        get { return canWalk; }
        set { canWalk = value; }
    }

    public float WanderingWalkSpeed
    {
        get { return wanderingWalkSpeed; }
    }

    public float ChasingWalkSpeed
    {
        get { return chasingWalkSpeed; }
    }

    public bool CameToEdge
    {
        get { return cameToEdge; }
    }

    public MoveCommand MoveCommand
    {
        get { return moveCommand; }
    }
    #endregion

    #region Unity Methods

    private void Awake()
    {
        AssingVariables();
        AssignCommands();
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;
        if (!canWalk) return;

        moveCommand.Execute(this.transform, walkValue);
        FlipWalkDirection();
    }
    #endregion

    #region Private Methods
    private void AssingVariables()
    {
        proceduralLegs = GetComponent<ProceduralLegs>();
        characterManager = GetComponent<CharacterManager>();
        enemyAIController = GetComponent<EnemyAIController>();
        obstacleBlock = GetComponent<ObstacleBlock>();
        enemyCollisionController = GetComponentInChildren<EnemyCollisionController>();

        walkSpeed = wanderingWalkSpeed;
        walkValue = 1;
        canWalk = true;
        cameToEdge = false;
    }

    private void AssignCommands()
    {
        moveCommand = new MoveCommand(proceduralLegs, walkSpeed * characterManager.DirectionMultiplier());
    }

    private void FlipWalkDirection()
    {
        if (IsInEdge() || obstacleBlock.HaveHitedObstacle())
        {
            walkValue *= -1;
            cameToEdge = true;
        }

        if (enemyCollisionController.ExitPatrolRegion)
        {
            enemyCollisionController.OffExitPatrolRegion();
            walkValue *= -1;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>Tells the enemy to stop walking, setting its walk value to 0.</summary>
    public void StopWalk()
    {
        walkValue = 0;
    }

    /// <summary>Tells the enemy to walk, setting its walk value to 1.</summary>
    public void ContinueWalkRight()
    {
        walkValue = 1;
    }

    /// <summary>Tells the enemy to walk, setting its walk value to 1.</summary>
    public void ContinueWalkLeft()
    {
        walkValue = -1;
    }

    /// <summary>Sets the walk speed of the enemy to the speed when wandering.</summary>
    public void WalkSpeedWandering()
    {
        walkSpeed = wanderingWalkSpeed;
        moveCommand.SetWalkSpeed(walkSpeed);
    }

    /// <summary>Sets the walk speed of the enemy to the speed when chasing.</summary>
    public void WalkSpeedChasing()
    {
        walkSpeed = chasingWalkSpeed;
        moveCommand.SetWalkSpeed(walkSpeed);
    }

    /// <summary>Sets the came to edge to false only.</summary>
    public void SetCameToEdge()
    {
        cameToEdge = false;
    }

    public MoveCommand GetMovementCommand()
    {
        return this.moveCommand;
    }

    public bool GetExitPatrolRegion()
    {
        return this.enemyCollisionController.ExitPatrolRegion;
    }

    /// <summary>Gets if the enemy is in a platform edge.</summary>
    public bool IsInEdge()
    {
        return !JumpUtils.MoreThenHalfLegsIsGrounded(characterManager.GroundChecks, characterManager.GroundCheckDistance, characterManager.GroundLayers);
    }
    #endregion
}