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

    [HeaderPlus(" ", "- WALK POSITIONS -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The local position that the target will be when walking")]
    [SerializeField] private Vector3 walkingTargetLocalPosition;
    [Tooltip("The speed that the target of the torso will move to the desired position when transition from/to walk")]
    [SerializeField] private float targetMoveSpeed;
    [Tooltip("The z rotation value that the head will perform while walking")]
    [SerializeField] private float walkingHeadLocalRotationValue;
    #endregion

    #region Private Vars
    private CharacterMovementState characterMovementState;
    private ProceduralLegs proceduralLegs;
    private CharacterManager characterManager;

    private MoveCommand moveCommand;

    private Vector3 idleTargetLocalPosition;

    private float idleHeadLocalRotationValue;
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
        idleHeadLocalRotationValue = headBone.localRotation.z;
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        var walkValue = moveCommand.GetXVelocity();

        switch (walkValue)
        {
            case 1f:
                MoveTarget(idleTargetLocalPosition);
                break;

            case > 1f:
                MoveTarget(walkingTargetLocalPosition, proceduralLegs.GetIsWalking());
                break;

            default:
                break;
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
    #endregion
}
