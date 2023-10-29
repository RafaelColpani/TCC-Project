using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using System;

[System.Serializable]
public class TailsTargets
{
    #region Inspector Vars
    [Tooltip("The target of the tail, that effector follows")]
    public Transform target;
    [Tooltip("The tail effector")]
    public Transform effector;
    [Tooltip("The position that the tail will aim while in idle animation.")]
    public Vector3[] idleMovementPositions;
    [Tooltip("The position that the tail will aim while in walk animation.")]
    public Vector3[] walkingMovementPositions;

    [Space(10)]

    [Tooltip("The position that the target will reach when ascending (jumping).")]
    public Vector3 ascendingLocalPosition;
    [Tooltip("The position that the target will reach when descending (falling).")]
    public Vector3 desendingLocalPosition;
    #endregion

    #region VARS
    [HideInInspector] public bool[] movingToIdle;
    [HideInInspector] public bool[] movingToWalk;
    [HideInInspector] public bool idleIsMovingForward;
    [HideInInspector] public bool walkIsMovingForward;
    #endregion

    #region GETTERS
    #endregion

    #region AUX
    public void MoveTarget(Vector3 finalPosition, float speed, bool canMove = true)
    {
        // is not moving any leg
        if (!canMove) return;

        var newPosition = Vector3.Lerp(target.localPosition, finalPosition, speed * Time.fixedDeltaTime);
        target.localPosition = newPosition;
    }
    #endregion

    #region IDLE & WALK
    public void ResetMovingTo(ref bool[] movingTo, Vector3[] positions)
    {
        bool[] aux = new bool[positions.Length];
        for (var i = 0; i < aux.Length; i++)
        {
            if (i == aux.Length - 1)
            { aux[i] = true; break; }

            else
                aux[i] = false;
        }

        movingTo = aux;
    }

    public void SetNewAnimationPosition(ref bool[] movingTo, ref bool isMovingForward)
    {
        for (var i = 0; i < movingTo.Length; i++)
        {
            if (!movingTo[i]) continue;

            if (i == movingTo.Length - 1)
                isMovingForward = false;

            else if (i == 0)
                isMovingForward = true;

            SwapAnimationPosition(ref movingTo, ref isMovingForward, i);
            break;
        }
    }

    private void SwapAnimationPosition(ref bool[] movingTo, ref bool isMovingForward, int index)
    {
        movingTo[index] = false;

        if (isMovingForward)
            movingTo[index + 1] = true;

        else
            movingTo[index - 1] = true;
    }
    #endregion
}

[RequireComponent(typeof(CharacterMovementState))]
public class ProceduralTail : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- TAIL TARGETS -", (int)HeaderPlusColor.green)]
    [SerializeField] private List<TailsTargets> tailsTargets;

    [HeaderPlus(" ", "- IDLE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The Speed that that targets of the tails will move in idle animation")]
    [SerializeField] private float idleAnimationSpeed;

    [HeaderPlus(" ", "- WALKING -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The Speed that that targets of the tails will go to the walking position")]
    [SerializeField] private float walkingAnimationSpeed;

    [HeaderPlus(" ", "- ASCENDING -", (int)HeaderPlusColor.red)]
    [Tooltip("The Speed that that targets of the tails will go to the ascending (jumping) position")]
    [SerializeField] private float ascendingAnimationSpeed;

    [HeaderPlus(" ", "- DESCENDING -", (int)HeaderPlusColor.magenta)]
    [Tooltip("The Speed that that targets of the tails will go to the descending (falling) position")]
    [SerializeField] private float descendingAnimationSpeed;
    #endregion

    #region Private VARs
    CharacterMovementState characterMovementState;
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterMovementState = GetComponent<CharacterMovementState>();

        foreach (var tail in tailsTargets)
        {
            tail.ResetMovingTo(ref tail.movingToIdle, tail.idleMovementPositions);
            tail.ResetMovingTo(ref tail.movingToWalk, tail.walkingMovementPositions);
        }
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        SetState();
    }
    #endregion

    #region Private Methods
    private void SetState()
    {
        switch (characterMovementState.MoveState)
        {
            case CharacterMovementState.MovementState.IDLE:
                IdleWalkAnimation(characterMovementState.MoveState);
                break;

            case CharacterMovementState.MovementState.WALKING:
                IdleWalkAnimation(characterMovementState.MoveState);
                break;

            default:
                JumpFallAnimation(characterMovementState.MoveState);
                break;
        }
    }

    #region IDLE & WALK
    private void IdleWalkAnimation(CharacterMovementState.MovementState moveState)
    {
        bool[] movingTo;
        Vector3[] positions;
        float speed;
        float distance;

        foreach (var tail in tailsTargets)
        {
            switch (moveState)
            {
                case CharacterMovementState.MovementState.IDLE:
                    tail.ResetMovingTo(ref tail.movingToWalk, tail.walkingMovementPositions);
                    movingTo = tail.movingToIdle;
                    positions = tail.idleMovementPositions;
                    speed = idleAnimationSpeed;
                    distance = 0.0001f;
                    break;

                default: // walking
                    tail.ResetMovingTo(ref tail.movingToIdle, tail.idleMovementPositions);
                    movingTo = tail.movingToWalk;
                    positions = tail.walkingMovementPositions;
                    speed = walkingAnimationSpeed;
                    distance = 0.05f;
                    break;
            }

            var goToPositionIndex = Array.IndexOf(movingTo, true);
            if (goToPositionIndex == -1) { Debug.LogError($"GOT INDEX -1 IN TAIL {moveState} ANIMATION"); return; }

            var targetsDistance = (tail.target.localPosition - positions[goToPositionIndex]).sqrMagnitude;

            tail.MoveTarget(positions[goToPositionIndex], speed);
            if (targetsDistance < distance)
            {
                switch (moveState)
                {
                    case CharacterMovementState.MovementState.IDLE:
                        tail.SetNewAnimationPosition(ref tail.movingToIdle, ref tail.idleIsMovingForward);
                        break;

                    default: // walking
                        tail.SetNewAnimationPosition(ref tail.movingToWalk, ref tail.walkIsMovingForward);
                        break;
                }
            }
        }
    }
    #endregion

    #region JUMP & FALL
    private void JumpFallAnimation(CharacterMovementState.MovementState moveState)
    {
        float speed = 0;
        Vector3 destination = Vector3.zero;

        foreach (var tail in tailsTargets)
        {
            tail.ResetMovingTo(ref tail.movingToIdle, tail.idleMovementPositions);
            tail.ResetMovingTo(ref tail.movingToWalk, tail.walkingMovementPositions);

            switch (moveState)
            {
                case CharacterMovementState.MovementState.ASCENDING:
                    speed = ascendingAnimationSpeed;
                    destination = tail.ascendingLocalPosition;
                    break;

                default: // DESCENDING
                    speed = descendingAnimationSpeed;
                    destination = tail.desendingLocalPosition;
                    break;
            }

            tail.MoveTarget(destination, speed);
        }
    }
    #endregion
    #endregion
}