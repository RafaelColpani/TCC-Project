using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using KevinCastejon.MoreAttributes;
using System.Linq;
using Unity.Burst.Intrinsics;

[System.Serializable]
public class ArmsTargets
{
    #region Inspector Vars
    [Tooltip("The target that he arm effector will follow")]
    public Transform effectorTarget;
    [Tooltip("The effector that will link the bones")]
    public Transform effector;
    [Tooltip("The position that the arm will aim while moving ahead")]
    public Transform aheadTarget;
    [Tooltip("The position that the arm will aim while moving behind the body")]
    public Transform behindTarget;
    [Tooltip("The position that the target will be reach when ascending (jumping).")]
    public Vector3 ascendingLocalPosition;
    [Tooltip("The height that the target will be, based on body position")]
    public float targetHeightOffset;
    [Tooltip("Each arm will move in a different direction, problably accordingly to the oposite leg, so it's " +
        "recommended to mark this as oposite to the same side leg (Example: if right leg is true, the right arm must be false)")]
    public bool isEven;
    #endregion

    #region Private Vars
    private Vector3 idlePosition;
    private Vector3 descendingPosition;

    private bool isMovingAhead;
    private bool isMovingBehind;
    #endregion

    #region Getters
    public Vector3 IdlePosition
    {
        get { return this.idlePosition; }
    }

    public Vector3 DescendingPosition
    {
        get { return this.descendingPosition; }
    }

    public bool IsMovingAhead
    {
        get { return this.isMovingAhead; }
    }

    public bool IsMovingBehind
    {
        get { return this.isMovingBehind; }
    }
    #endregion

    #region Setters
    public void SetIdlePositionByEffectorTarget()
    {
        this.idlePosition = effectorTarget.localPosition;
    }

    public void SetDescendingPosition(float value)
    {
        this.descendingPosition = new Vector3(effectorTarget.localPosition.x,
                                              effectorTarget.localPosition.y + value,
                                              effectorTarget.localPosition.z);
    }

    public void SetIsMovingAhead(bool value = true)
    {
        this.isMovingAhead = value;
    }

    public void SetIsMovingBehind(bool value = true)
    {
        this.isMovingBehind = value;
    }
    #endregion
}

[RequireComponent(typeof(ProceduralLegs))]
[RequireComponent(typeof(CharacterMovementState))]
public class ProceduralArms : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- ARMS TARGETS -", (int)HeaderPlusColor.green)]
    [SerializeField] private List<ArmsTargets> armsTargets;

    [HeaderPlus(" ", "- MOVE & IDLE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The height the arc will realize when walking.")]
    [SerializeField] private float armArcHeight;
    [Tooltip("The speed the arc will realize when walking.")]
    [SerializeField] private float armArcSpeed;
    [Tooltip("The overall speed that the arm will realize its walking movement.")]
    [SerializeField] private float armMoveSpeed;
    [Tooltip("The speed that the arms will go back to the idle position.")]
    [SerializeField] private float backToIdleSpeed;

    [HeaderPlus(" ", "- ASCENDING -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The speed that arms will go to the ascending position.")]
    [SerializeField] private float ascendingSpeed;

    [HeaderPlus(" ", "- DESCENDING -", (int)HeaderPlusColor.magenta)]
    [Tooltip("The value that will be added to the y position of the target.")]
    [SerializeField] private float descendingValue;
    [Tooltip("The speed that arms will go to the descending position.")]
    [SerializeField] private float descendingSpeed;
    #endregion

    #region Private VARs
    private CharacterManager characterManager;
    private ProceduralLegs proceduralLegs;
    private CharacterMovementState characterMovementState;

    private Transform body;

    private float lerpArm = 0;
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        proceduralLegs = GetComponent<ProceduralLegs>();
        characterMovementState = GetComponent<CharacterMovementState>();

        body = characterManager.Body;

        foreach (var arm in armsTargets)
        {
            arm.SetIdlePositionByEffectorTarget();
            arm.SetDescendingPosition(descendingValue);
        }
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        var moveState = characterMovementState.MoveState;

        switch (moveState)
        {
            case CharacterMovementState.MovementState.IDLE:
                ArmsIdlePosition();
                break;

            case CharacterMovementState.MovementState.WALKING:
                MoveWalkingArms();
                break;

            case CharacterMovementState.MovementState.ASCENDING:
                ArmsAscendingPosition();
                break;

            case CharacterMovementState.MovementState.DESCENDING:
                ArmsDescendingPosition();
                break;
        }
    }
    #endregion

    #region Private Methods
    private void ArmsIdlePosition()
    {
        foreach (var arm in armsTargets)
        {
            if (arm.effectorTarget.localPosition == arm.IdlePosition) continue;

            var targetsDistance = (arm.effectorTarget.localPosition - arm.IdlePosition).sqrMagnitude;
            var newPosition = Vector3.Slerp(arm.effectorTarget.localPosition,
                                                            arm.IdlePosition,
                                                            backToIdleSpeed * Time.fixedDeltaTime);
            
            arm.effectorTarget.localPosition = newPosition;

            if (targetsDistance < 0.01f)
            {
                arm.effectorTarget.localPosition = arm.IdlePosition;
            }
        }
    }

    private void MoveWalkingArms()
    {
        // is not moving any leg
        if (!proceduralLegs.GetIsWalking()) return;

        var oddIsWalking = proceduralLegs.OddIsWalking;

        foreach (var arm in armsTargets)
        {
            // the opposite arm move ahead
            var moveAhead = (oddIsWalking && arm.isEven) || (!oddIsWalking && !arm.isEven);

            if (!ArmIsMovingInRightDirection(arm, moveAhead))
            {
                if (moveAhead)
                {
                    arm.SetIsMovingAhead(true);
                    arm.SetIsMovingBehind(false);
                }
                    
                else
                {
                    arm.SetIsMovingAhead(false);
                    arm.SetIsMovingBehind(true);
                }

                lerpArm = 0;
            }

            float height = armArcHeight;
            float arc = Mathf.Sin(lerpArm * Mathf.PI) * height;
            lerpArm += Time.fixedDeltaTime * armArcSpeed;


            var newPosition = Vector3.Lerp( arm.effectorTarget.position,
                                            moveAhead ? arm.aheadTarget.position : arm.behindTarget.position,
                                            armMoveSpeed * Time.deltaTime);

            if (lerpArm < 1)
            {
                if (moveAhead)
                    newPosition.y += arc;
                else
                    newPosition.y -= arc;
            }

            else
                newPosition = moveAhead ? arm.aheadTarget.position : arm.behindTarget.position;

            arm.effectorTarget.position = newPosition;
        }
    }

    private void ArmsAscendingPosition()
    {
        foreach (var arm in armsTargets)
        {
            var newPosition = Vector3.Lerp(arm.effectorTarget.localPosition, arm.ascendingLocalPosition, ascendingSpeed * Time.deltaTime);
            arm.effectorTarget.localPosition = newPosition;
        }
    }

    private void ArmsDescendingPosition()
    {
        foreach (var arm in armsTargets)
        {
            var newPosition = Vector3.Lerp(arm.effectorTarget.localPosition, arm.DescendingPosition, descendingSpeed * Time.deltaTime);
            arm.effectorTarget.localPosition = newPosition;
        }
    }

    private bool ArmIsMovingInRightDirection(ArmsTargets arm, bool moveAhead)
    {
        if (moveAhead)
            return arm.IsMovingAhead == true;

        else
            return arm.IsMovingBehind == true;
    }
    #endregion
}
