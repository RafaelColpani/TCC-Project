using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using KevinCastejon.MoreAttributes;
using System.Linq;

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
    [Tooltip("The height that the target will be, based on body position")]
    public float targetHeightOffset;
    [Tooltip("Each arm will move in a different direction, problably accordingly to the oposite leg, so it's " +
        "recommended to mark this as oposite to the same side leg (Example: if right leg is true, the right arm must be false)")]
    public bool isEven;
    #endregion

    #region Private Vars
    private bool isMovingAhead;
    private bool isMovingBehind;
    #endregion

    #region Getters
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
public class ProceduralArms : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- ARMS TARGETS -", (int)HeaderPlusColor.green)]
    [SerializeField] private List<ArmsTargets> armsTargets;

    [HeaderPlus(" ", "- ARMS -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The height the arc will realize when walking.")]
    [SerializeField] private float armArcHeight;
    [Tooltip("The speed the arc will realize when walking.")]
    [SerializeField] private float armArcSpeed;
    [Tooltip("The overall speed that the arm will realize its walking movement.")]
    [SerializeField] private float armMoveSpeed;
    #endregion

    #region Private VARs
    private CharacterManager characterManager;
    private ProceduralLegs proceduralLegs;

    private Transform body;

    private float lerpArm = 0;
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        proceduralLegs = GetComponent<ProceduralLegs>();

        body = characterManager.Body;
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

        MoveArms();
    }
    #endregion

    #region Private Methods
   private void MoveArms()
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

    private bool ArmIsMovingInRightDirection(ArmsTargets arm, bool moveAhead)
    {
        if (moveAhead)
        {
            return arm.IsMovingAhead == true;
        }

        else
        {
            return arm.IsMovingBehind == true;
        }
        
    }
    #endregion
}
