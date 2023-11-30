using System;
using System.Collections.Generic;
using UnityEngine;
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
    [Tooltip("The position that the arm will aim while in idle animation.")]
    public Vector3[] idleMovementPositions;
    [Tooltip("The position that the target will reach when ascending (jumping).")]
    public Vector3 ascendingLocalPosition;
    [Tooltip("Each arm will move in a different direction, problably accordingly to the oposite leg, so it's " +
        "recommended to mark this as oposite to the same side leg (Example: if right leg is true, the right arm must be false)")]
    public bool isEven;
    #endregion

    #region Private Vars
    private Vector3 idlePosition;
    private Vector3 descendingPosition;

    private bool isMovingAhead;
    private bool isMovingBehind;
    private bool[] movingToIdle;
    private bool idleIsMovingForward;
    private bool reachedMaxPosIdleAnim;
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

    public bool[] MovingToIdle
    {
        get { return movingToIdle; }
    }

    public bool ReachedMaxPosIdleAnim
    {
        get { return reachedMaxPosIdleAnim; }
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

    public void SetMovingToIdle(bool[] values)
    {
        this.movingToIdle = values;
    }

    public void SetReachedMaxPosIdleAnim(bool value)
    {
        this.reachedMaxPosIdleAnim = value;
    }

    public void ResetMovingToIdle(bool followTogether)
    {
        if (!followTogether)
        {
            for (int i = 0; i < this.movingToIdle.Length; i++)
            {
                if (this.isEven && i == this.movingToIdle.Length - 1)
                {
                    movingToIdle[i] = true;
                    idleIsMovingForward = false;
                }

                else if (!this.isEven && i == 0)
                {
                    movingToIdle[i] = true;
                    idleIsMovingForward = true;
                }

                else
                    movingToIdle[i] = false;
            }
        }

        else
        {
            for (int i = 0; i < this.movingToIdle.Length; i++)
            {
                if (i == 0)
                {
                    movingToIdle[i] = true;
                    idleIsMovingForward = true;
                }

                else
                    movingToIdle[i] = false;
            }
        }
    }

    public void SetNewIdleAnimationPosition()
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
    }

    private void SwapIdleAnimationPosition(int index)
    {
        movingToIdle[index] = false;

        if (idleIsMovingForward)
            movingToIdle[index + 1] = true;

        else
            movingToIdle[index - 1] = true;
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

    [HeaderPlus(" ", "- IDLE -", (int)HeaderPlusColor.red)]
    [Tooltip("The speed that the arms will go back to the idle position.")]
    [SerializeField] private float backToIdleSpeed;
    [Tooltip("The speed that the arms will move to perform the idle animation.")]
    [SerializeField] private float idleAnimationSpeed;
    [Tooltip("The curve that arm will move in the idle animaion between targets.")]
    [SerializeField] private AnimationCurve idleAnimationCurve;
    [Tooltip("Tells if the arms will begun the idle animation in same direction.")]
    [SerializeField] private bool idleAnimationFollowTogether;
    [Tooltip("Tells if the arms will move accordingly with the torso.")]
    [SerializeField] private bool followTorsoAnimation;

    [HeaderPlus(" ", "- WALKING -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The height the arc will realize when walking.")]
    [SerializeField] private float armArcHeight;
    [Tooltip("The speed the arc will realize when walking.")]
    [SerializeField] private float armArcSpeed;
    [Tooltip("The overall speed that the arm will realize its walking movement.")]
    [SerializeField] private float armMoveSpeed;
    

    [HeaderPlus(" ", "- ASCENDING -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The speed that arms will go to the ascending position.")]
    [SerializeField] private float ascendingSpeed;

    [HeaderPlus(" ", "- DESCENDING -", (int)HeaderPlusColor.magenta)]
    [Tooltip("The value that will be added to the y position of the target.")]
    [SerializeField] private float descendingValue;
    [Tooltip("The speed that arms will go to the descending position.")]
    [SerializeField] private float descendingSpeed;

    [HeaderPlus(" ", "- CARRYING -", (int)HeaderPlusColor.white)]
    [Tooltip("The position that both targets and carrying object will be when carrying something")]
    [SerializeField] private Vector3 carryingPosition;
    [Tooltip("The speed that both targets and carrying object will go to the carrying position")]
    [SerializeField] private float carrySpeed;
    #endregion

    #region Private VARs
    private CharacterManager characterManager;
    private ProceduralLegs proceduralLegs;
    private CharacterMovementState characterMovementState;
    private ProceduralTorso proceduralTorso;
    private GameObject carryingObject;
    private Transform carryingObjectParent;

    private float lerpArm = 0;

    private bool startIdleAnimation = true;
    private bool isCarryingObject = false;
    private bool armsIsCarrying = false;
    #endregion

    #region Getters
    public bool IsCarryingObject { get { return isCarryingObject; } }

    public bool ArmsIsCarrying { get { return armsIsCarrying; } set { armsIsCarrying = value; } }
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        proceduralLegs = GetComponent<ProceduralLegs>();
        characterMovementState = GetComponent<CharacterMovementState>();

        foreach (var arm in armsTargets)
        {
            arm.SetIdlePositionByEffectorTarget();
            arm.SetDescendingPosition(descendingValue);

            bool[] aux = new bool[arm.idleMovementPositions.Count()];
            for (var i = 0; i < aux.Count(); i++)
                aux[i] = false;
            arm.SetMovingToIdle(aux);
            arm.ResetMovingToIdle(idleAnimationFollowTogether);
        }

        if (followTorsoAnimation)
            proceduralTorso = GetComponent<ProceduralTorso>();
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        SetArmsState();
    }
    #endregion

    #region Private Methods
    private void SetArmsState()
    {
        if (!isCarryingObject)
        {
            var moveState = characterMovementState.MoveState;

            switch (moveState)
            {
                case CharacterMovementState.MovementState.IDLE:
                    if (!startIdleAnimation)
                        ArmsIdlePosition();
                    else
                        IdleAnimation();
                    break;

                case CharacterMovementState.MovementState.WALKING:
                    ResetIdleAnimation();
                    MoveWalkingArms();
                    break;

                case CharacterMovementState.MovementState.ASCENDING:
                    ResetIdleAnimation();
                    ArmsAscendingPosition();
                    break;

                case CharacterMovementState.MovementState.DESCENDING:
                    ResetIdleAnimation();
                    ArmsDescendingPosition();
                    break;
            }
        }

        else
        {
            CarryingObjectPosition();
        }
    }

    #region IDLE
    private void ArmsIdlePosition()
    {
        foreach (var arm in armsTargets)
        {
            if (arm.effectorTarget.localPosition == arm.IdlePosition) continue;

            arm.SetReachedMaxPosIdleAnim(false);
            var targetsDistance = (arm.effectorTarget.localPosition - arm.IdlePosition).sqrMagnitude;
            var newPosition = Vector3.Slerp(arm.effectorTarget.localPosition,
                                                            arm.IdlePosition,
                                                            backToIdleSpeed * Time.fixedDeltaTime);
            
            arm.effectorTarget.localPosition = newPosition;

            if (targetsDistance < 0.001f)
            {
                arm.effectorTarget.localPosition = arm.IdlePosition;
                arm.ResetMovingToIdle(idleAnimationFollowTogether);
                startIdleAnimation = true;
            }
        }
    }

    private void IdleAnimation()
    {
        int armsToTorsoCount = 0;

        foreach (var arm in armsTargets)
        {
            var goToPositionIndex = Array.IndexOf(arm.MovingToIdle, true);
            if (goToPositionIndex == -1) { Debug.LogError("GOT INDEX -1 IN ARMS IDLE ANIMATION"); continue; }

            var targetsDistance = (arm.effectorTarget.localPosition - arm.idleMovementPositions[goToPositionIndex]).sqrMagnitude;
            var time = idleAnimationSpeed * Time.fixedDeltaTime;
            var newPosition = Vector3.Lerp(arm.effectorTarget.localPosition,
                                                           arm.idleMovementPositions[goToPositionIndex],
                                                           idleAnimationCurve.Evaluate(time));
            arm.effectorTarget.localPosition = newPosition;

            // arms will change direction regardless of the torso animation
            if (!followTorsoAnimation)
            {
                if (targetsDistance < 0.001f)
                {
                    arm.SetReachedMaxPosIdleAnim(true);
                    bool allArmsReached = false;

                    for (int i = 0; i < armsTargets.Count(); i++)
                    {
                        if (armsTargets[i] == arm) continue;
                        if (!armsTargets[i].ReachedMaxPosIdleAnim) break;

                        allArmsReached = true;
                    }

                    if (!allArmsReached) continue;

                    arm.SetNewIdleAnimationPosition();
                    continue;
                }

                arm.SetReachedMaxPosIdleAnim(false);
            }
            
            // arms will change direction along with the torso animation
            else
            {
                if (proceduralTorso.GetArmsIsFollowing()) continue;

                arm.SetNewIdleAnimationPosition();
                armsToTorsoCount++;

                if (armsToTorsoCount < armsTargets.Count()) continue;

                proceduralTorso.EnableArmsIsFollowingFlag();
                armsToTorsoCount = 0;
            }
        }
    }
    #endregion

    #region WALKING
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
    #endregion

    #region JUMPING
    private void ArmsAscendingPosition()
    {
        foreach (var arm in armsTargets)
        {
            var newPosition = Vector3.Lerp(arm.effectorTarget.localPosition, arm.ascendingLocalPosition, ascendingSpeed * Time.deltaTime);
            arm.effectorTarget.localPosition = newPosition;
        }
    }
    #endregion

    #region FALLING
    private void ArmsDescendingPosition()
    {
        foreach (var arm in armsTargets)
        {
            var newPosition = Vector3.Lerp(arm.effectorTarget.localPosition, arm.DescendingPosition, descendingSpeed * Time.deltaTime);
            arm.effectorTarget.localPosition = newPosition;
        }
    }
    #endregion

    #region CARRYING
    private void CarryingObjectPosition()
    {
        foreach (var arm in armsTargets)
        {
            var newPosition = Vector3.Lerp(arm.effectorTarget.localPosition, carryingPosition, carrySpeed * Time.deltaTime);
            arm.effectorTarget.localPosition = newPosition;
        }

        var newCarriedObjectPosition = Vector3.Lerp(carryingObject.transform.localPosition, carryingPosition, carrySpeed * Time.deltaTime);
        carryingObject.transform.localPosition = newCarriedObjectPosition;
    }
    #endregion

    #region AUX
    private bool ArmIsMovingInRightDirection(ArmsTargets arm, bool moveAhead)
    {
        if (moveAhead)
            return arm.IsMovingAhead == true;

        else
            return arm.IsMovingBehind == true;
    }

    private void ResetIdleAnimation()
    {
        startIdleAnimation = false;

        foreach (var arm in armsTargets)
        {
            arm.SetReachedMaxPosIdleAnim(false);
            arm.ResetMovingToIdle(followTorsoAnimation);
        }
    }
    #endregion
    #endregion

    #region Public Methods
    public void CarryObject(GameObject carriedObject)
    {
        isCarryingObject = true;
        startIdleAnimation = false;
        armsIsCarrying = true;
        carryingObject = carriedObject;
        carryingObjectParent = carryingObject.transform.parent;
        carryingObject.transform.SetParent(characterManager.Body);

        if (carriedObject.GetComponent<Rigidbody2D>())
            carriedObject.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void DropObject(GameObject carriedObject = null)
    {
        if (!isCarryingObject) return;
        if (carryingObject != carriedObject && carriedObject != null) return;
        if (armsIsCarrying)
        {
            armsIsCarrying = false;
            return;
        }

        isCarryingObject = false;
        carryingObject.transform.parent = carryingObjectParent;

        if (carryingObject.GetComponent<Rigidbody2D>())
            carryingObject.GetComponent<Rigidbody2D>().isKinematic = false;

        carryingObject = null;
        carryingObjectParent = null;
    }
    #endregion
}
