using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using KevinCastejon.MoreAttributes;
using System.Linq;

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

    [HeaderPlus(" ", "- TORSO -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The local position that the target will be when walking")]
    [SerializeField] private Vector3 walkingTargetLocalPosition;
    [Tooltip("The speed that the target of the torso will move to the desired position when transition from/to walk")]
    [SerializeField] private float targetMoveSpeed;

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
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterMovementState = GetComponent<CharacterMovementState>();
        proceduralLegs = GetComponent<ProceduralLegs>();
        characterManager = GetComponent<CharacterManager>();

        if (characterManager.CommandsByInputHandler)
            moveCommand = GetComponent<InputHandler>().GetMovementCommand();

        idleTargetLocalPosition = target.localPosition;
        idleHeadLocalRotation = headBone.localRotation;
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        var walkValue = moveCommand.CurrentSpeed;
        var state = characterMovementState.MoveState;

        if (state == CharacterMovementState.MovementState.ASCENDING || state == CharacterMovementState.MovementState.DESCENDING)
        {
            MoveTarget(idleTargetLocalPosition);
            RotateHead(idleHeadLocalRotation);
        }

        else
        {
            switch (walkValue)
            {
                case 0f:
                    MoveTarget(idleTargetLocalPosition);
                    RotateHead(idleHeadLocalRotation);
                    break;

                default:
                    MoveTarget(walkingTargetLocalPosition, proceduralLegs.GetIsWalking());
                    RotateHead(walkingHeadLocalRotation, proceduralLegs.GetIsWalking());
                    break;
            }
        }
    }
    #endregion

    #region Private Methods
    private void MoveTarget(Vector3 finalPosition, bool canMove = true)
    {
        // is not moving any leg
        if (!canMove) return;

        var newPosition = Vector3.Lerp(target.localPosition, finalPosition, targetMoveSpeed * Time.fixedDeltaTime);
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
}
