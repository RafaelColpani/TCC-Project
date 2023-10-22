using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using KevinCastejon.MoreAttributes;
using System.Linq;
using System;
using Unity.Burst.Intrinsics;

[RequireComponent(typeof(CharacterMovementState))]
[RequireComponent(typeof(ProceduralLegs))]
[RequireComponent(typeof(CharacterManager))]
public class ProceduralTorso : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- BONES -", (int)HeaderPlusColor.green)]
    [Tooltip("The target that control de IK of the torso")]
    [SerializeField] private Transform target;
    [Tooltip("The Transform where is located the bone of the head of the character")]
    [SerializeField] private Transform headBone;

    [HeaderPlus(" ", "- GENERAL -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The local position that the target will be when walking")]
    [SerializeField] private Vector3 walkingTargetLocalPosition;
    [Tooltip("The speed that the target of the torso will move to the desired position when transition from/to walk")]
    [SerializeField] private float targetMoveSpeed;

    [HeaderPlus(" ", "- IDLE -", (int)HeaderPlusColor.red)]
    [Tooltip("The positions that the target will move in idle animation")]
    [SerializeField] private Vector3[] targetsIdleAnimation;
    [Tooltip("The speed that the target will move to the positions of the idle animation")]
    [SerializeField] private float idleAnimationSpeed;

    [HeaderPlus(" ", "- HEAD -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The z rotation value that the head will perform while walking")]
    [SerializeField] private Quaternion walkingHeadLocalRotation;
    [Tooltip("The speed that the head will rotate to the desired quaternion when transition from/to walk")]
    [SerializeField] private float headRotationSpeed;
    #endregion

    #region Private Vars
    private CharacterMovementState characterMovementState;
    private ProceduralLegs proceduralLegs;
    private CharacterManager characterManager;

    private MoveCommand moveCommand;

    private Vector3 idleTargetLocalPosition;

    private Quaternion idleHeadLocalRotation;

    private bool isInIdle = true;
    private bool idleIsMovingForward = true;
    private bool[] movingToIdle;
    private bool armsIsFollowing = true;
    private bool canMoveTarget = true;
    #endregion

    public bool CanMoveTarget
    {
        get { return canMoveTarget; }
        set { canMoveTarget = value; }
    }

    #region Unity Methods
    private void Start()
    {
        characterMovementState = GetComponent<CharacterMovementState>();
        proceduralLegs = GetComponent<ProceduralLegs>();
        characterManager = GetComponent<CharacterManager>();

        if (characterManager.CommandsByInputHandler)
            moveCommand = GetComponent<InputHandler>().GetMovementCommand();
        else if (GetComponent<ChickenFruitFollow>() != null)
            moveCommand = GetComponent<ChickenFruitFollow>().GetMoveCommand();

        idleTargetLocalPosition = target.localPosition;
        idleHeadLocalRotation = headBone.localRotation;

        ResetIdleAnimationPosition();
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        SetTorsoState();
    }
    #endregion

    #region Private Methods
    private void SetTorsoState()
    {
        var walkValue = moveCommand.CurrentSpeed;
        var state = characterMovementState.MoveState;

        // if is jumping or falling, goes to initial position
        if (state == CharacterMovementState.MovementState.ASCENDING || state == CharacterMovementState.MovementState.DESCENDING)
        {
            isInIdle = true;
            if (canMoveTarget)
                MoveTarget(idleTargetLocalPosition, targetMoveSpeed);

            RotateHead(idleHeadLocalRotation);
            ResetIdleAnimationPosition();
        }

        else
        {
            switch (walkValue)
            {
                // is idle
                case 0f:
                    TorsoIdleAnimation();
                    break;

                // is walking
                default:
                    isInIdle = false;
                    if (canMoveTarget)
                        MoveTarget(walkingTargetLocalPosition, targetMoveSpeed, proceduralLegs.GetIsWalking());

                    RotateHead(walkingHeadLocalRotation, proceduralLegs.GetIsWalking());
                    ResetIdleAnimationPosition();
                    break;
            }
        }
    }

    #region BONES MOVEMENT
    public void MoveTarget(Vector3 finalPosition, float speed, bool canMove = true)
    {
        // is not moving any leg
        if (!canMove) return;

        var newPosition = Vector3.Lerp(target.localPosition, finalPosition, speed * Time.fixedDeltaTime);
        target.localPosition = newPosition;
    }

    private void RotateHead(Quaternion finalRotation, bool canMove = true)
    {
        // is not moving any leg
        if (!canMove) return;

        var newRotation = Quaternion.Slerp(headBone.localRotation, finalRotation, headRotationSpeed * Time.fixedDeltaTime);
        headBone.localRotation = newRotation;
    }
    #endregion

    #region IDLE
    private void TorsoIdleAnimation()
    {
        // torso goes to initial position if was walking
        if (!isInIdle)
        {
            if (canMoveTarget)
                MoveTarget(idleTargetLocalPosition, targetMoveSpeed);
            var targetDistance = (target.localPosition - idleTargetLocalPosition).sqrMagnitude;
            if (targetDistance < 0.001f)
                isInIdle = true;
        }

        // torso in idle animation
        else
        {
            var goToPositionIndex = Array.IndexOf(movingToIdle, true);
            if (goToPositionIndex == -1) { Debug.LogError("GOT INDEX -1 IN TORSO IDLE ANIMATION"); return; }

            var targetsDistance = (target.localPosition - targetsIdleAnimation[goToPositionIndex]).sqrMagnitude;
            if (canMoveTarget)
                MoveTarget(targetsIdleAnimation[goToPositionIndex], idleAnimationSpeed);
            if (targetsDistance < 0.0001f)
            {
                SetNewIdleAnimationPosition();
            }
        }

        RotateHead(idleHeadLocalRotation);
    }

    private void SetNewIdleAnimationPosition()
    {
        for (var i = 0; i < movingToIdle.Length; i++)
        {
            if (!movingToIdle[i]) continue;

            if (i == movingToIdle.Length - 1)
                idleIsMovingForward = false;

            else if (i == 0)
                idleIsMovingForward = true;

            SwapIdleAnimationPosition(i);
            break;
        }

        armsIsFollowing = false;
    }

    private void SwapIdleAnimationPosition(int index)
    {
        movingToIdle[index] = false;

        if (idleIsMovingForward)
            movingToIdle[index + 1] = true;

        else
            movingToIdle[index - 1] = true;
    }

    private void ResetIdleAnimationPosition()
    {
        bool[] aux = new bool[targetsIdleAnimation.Length];
        for (var i = 0; i < aux.Length; i++)
        {
            if (i == aux.Length - 1)
            { aux[i] = true; break; }

            else
                aux[i] = false;
        }

        movingToIdle = aux;
    }
    #endregion
    #endregion

    #region Public Methods
    public bool GetArmsIsFollowing()
    {
        return armsIsFollowing;
    }

    public void EnableArmsIsFollowingFlag()
    {
        armsIsFollowing = true;
    }

    public Transform GetTarget()
    {
        return this.target;
    }
    #endregion
}
