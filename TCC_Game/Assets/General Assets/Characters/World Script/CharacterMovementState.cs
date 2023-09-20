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

    #region Vars
    [ReadOnly] [SerializeField]
    private MovementState moveState = MovementState.IDLE;

    private ProceduralLegs proceduralLegs;
    private GravityController gravityController;
    #endregion

    #region Getters
    public MovementState MoveState { get { return this.moveState; } }
    #endregion

    #region Unity Methods
    private void Start()
    {
        proceduralLegs = GetComponent<ProceduralLegs>();
        gravityController = GetComponent<GravityController>();
    }

    private void Update()
    {
        this.moveState = SetMovementState();
    }
    #endregion

    #region Private Methods
    private MovementState SetMovementState()
    {
        if (proceduralLegs.GetIsWalking())
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
