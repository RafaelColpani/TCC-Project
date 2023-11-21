using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(ProceduralLegs))]
[RequireComponent(typeof(GravityController))]
public class CharacterMovementState : MonoBehaviour
{
    public enum MovementState
    {
        IDLE, WALKING, ASCENDING, DESCENDING
    }

    [Tooltip("Tells if will say its wslking if legs is moving, if not, will get move commando velocity.")]
    [SerializeField] bool getWalkingByLegs = true;

    #region Vars
    [ReadOnly] [SerializeField]
    private MovementState moveState = MovementState.IDLE;

    private ProceduralLegs proceduralLegs;
    private GravityController gravityController;
    private MoveCommand moveCommand;
    #endregion

    #region Getters
    public MovementState MoveState { get { return this.moveState; } }
    #endregion

    #region Unity Methods
    private void Start()
    {
        proceduralLegs = GetComponent<ProceduralLegs>();
        gravityController = GetComponent<GravityController>();

        if (GetComponent<InputHandler>())
            moveCommand = GetComponent<InputHandler>().GetMovementCommand();
        else if (GetComponent<ChickenFruitFollow>() != null)
            moveCommand = GetComponent<ChickenFruitFollow>().GetMoveCommand();
        else if (GetComponent<GuidePlayerAI>() != null)
            moveCommand = GetComponent<GuidePlayerAI>().GetMoveCommand();
        else if (GetComponent<EnemyCommands>() != null)
            moveCommand = GetComponent<EnemyCommands>().GetMovementCommand();
    }

    private void Update()
    {
        if (PauseController.GetIsPaused()) return;
        this.moveState = SetMovementState();
    }
    #endregion

    #region Private Methods
    private MovementState SetMovementState()
    {
        if (proceduralLegs.GetIsWalking() && getWalkingByLegs)
            return MovementState.WALKING;

        else if (!getWalkingByLegs && Mathf.Abs(moveCommand.CurrentSpeed) > 0)
            return MovementState.WALKING;

        else if (gravityController.Velocity.y > 0)
            return MovementState.ASCENDING;

        else if (gravityController.Velocity.y < 0)
            return MovementState.DESCENDING;

        else
            return MovementState.IDLE;
    }
    #endregion
}
