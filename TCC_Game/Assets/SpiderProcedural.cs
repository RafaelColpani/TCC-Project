using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using KevinCastejon.MoreAttributes;

[System.Serializable]
public class ObjectTargets
{
    [Tooltip("The Target that follows among the body. Must be an object INSIDE the sprite parent.")]
    public Transform bodyTarget;
    [Tooltip("The target that the effector is following. Must be an object OUTSIDE the sprite parent.")]
    public Transform effectorTarget;
    [Tooltip("The target that will follow the bodyTarget with an offsite on the x axis. Must be an abject INSIDE the sprite parent.")]
    public Transform finalTarget;
    [Tooltip("The GameObject acting as the effector. Must be a child of the final bone of a leg.")]
    public Transform effector;
    [Tooltip("The legs will move accordingly to its timing. Even legs will not walk while Odds legs are walking, and vice versa.")]
    public bool isEven;

    private float stepTime;
    private bool isMoving;

    #region Getters & Setters
    [HideInInspector] public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    [HideInInspector] public float StepTime
    {
        get { return stepTime; }
    }
    #endregion

    #region Public Methods
    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }

    public void IncrementStepTime(float value)
    {
        stepTime += value;
    }

    public void ResetStepTime()
    {
        stepTime = 0;
    }
    #endregion
}

[RequireComponent(typeof(GravityController))]
[RequireComponent(typeof(IKManager2D))]
public class SpiderProcedural : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- TARGETS OBJECTS -", (int)HeaderPlusColor.green)]
    [SerializeField] private List<ObjectTargets> targets;

    [HeaderPlus(" ", "- DISTANCES -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The distance that the targets will be at the raycasted collider.")]
    [SerializeField] private float hoverRayDistance;
    [Tooltip("The maximum distance that the effectorTarget has to be from the bodyTarget to move the leg. Distance calculated with sqrMagnitude.")]
    [SerializeField] private float maxSqrTargetDistance;
    [Tooltip("The maximum distance that the raycast of the targets will detect collisions.")]
    [SerializeField] private float groundRaycastDistance;

    [HeaderPlus(" ", "- LEGS -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The speedthat the effectorTarget will move to it's destination when moving (finalTarget).")]
    [SerializeField] private float legStepSpeed;
    [Tooltip("The height of the arc that the leg makes when moving.")]
    [SerializeField] private float legArcHeight;
    [Tooltip("The speed of the arc that the leg makes when moving.")]
    [SerializeField] private float legArcSpeed;
    [Tooltip("The offset in the x axis that the finalTarget makes from the position of the bodyTarget.")]
    [SerializeField] private float xFinalTargetOffest;

    [HeaderPlus(" ", "- BODY -", (int)HeaderPlusColor.magenta)]
    [Tooltip("The object that parents all bones objects. Must be an empty object holding the bones objects as a parent.")]
    [SerializeField] private Transform body;
    [Tooltip("The offset that the body makes in the y axis.")]
    [SerializeField] private float bodyPositionOffset;

    [HeaderPlus(" ", "- ROTATION -", (int)HeaderPlusColor.red)]
    [Tooltip("Tells if the body will rotate accordingly to the position of the legs. Better used with a spider like animal for example.")]
    [SerializeField] private bool makeRotation;

    [HeaderPlus(" ", "- GROUND -", (int)HeaderPlusColor.white)]
    [Tooltip("Says what layers the targets will raycast to.")]
    [SerializeField] private LayerMask targetsDetections;
    [Tooltip("The transform in the position that will check if the object is grounded.")]
    [SerializeField] private Transform groundCheck;
    [Tooltip("The radius of the circle that will detect the ground from the checkGround Transform position.")]
    [SerializeField] private float groundCheckRadius;
    #endregion

    #region private VARs
    private GravityController gravityController;
    private IKManager2D ikManager;

    private bool evenIsWalking = false;
    private bool oddIsWalking = false;
    private bool proceduralIsOn = true;

    private float previousXBodyPosition;
    private float lerpLeg = 0;
    #endregion

    #region Public VARs
    public bool ProceduralIsOn
    {
        get { return this.proceduralIsOn; }
        set { this.proceduralIsOn = value; }
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        gravityController = GetComponent<GravityController>();
        ikManager = GetComponent<IKManager2D>();
    }

    private void Start()
    {
        foreach (var target in targets)
        {
            target.ResetStepTime();
            target.SetIsMoving(false);
        }

        previousXBodyPosition = body.position.x;
        ikManager.enabled = true;
    }

    private void FixedUpdate()
    {
        TargetsGroundHeight();

        if (!JumpUtils.IsGrounded(groundCheck, groundCheckRadius, targetsDetections) || !proceduralIsOn)
        {
            gravityController.SetIsOn(true);
            //ikManager.enabled = false;
            return;
        }
        gravityController.SetIsOn(false);
        proceduralIsOn = true;
        //ikManager.enabled = true;
        MoveLegs();
        MoveFinalTargets();
        CalculateBodyPosition();

        if (makeRotation)
            CalculateBodyRotation();
    }
    #endregion

    #region Private Methods

    #region Procedural Flow
    private void TargetsGroundHeight()
    {
        RaycastHit2D localHit;
        RaycastHit2D finalHit;

        foreach (var target in targets)
        {
            if (gravityController.GetIsOn())
            {
                target.effectorTarget.position = target.bodyTarget.position;
            }

            localHit = Physics2D.Raycast(target.bodyTarget.position, -Vector2.up, gravityController.GetIsOn()? Mathf.Infinity : groundRaycastDistance, targetsDetections);
            finalHit = Physics2D.Raycast(target.finalTarget.position, -Vector2.up, gravityController.GetIsOn() ? Mathf.Infinity : groundRaycastDistance, targetsDetections);

            if (localHit.collider == null)
                continue;

            Vector3 localRayPoint = localHit.point;
            Vector3 finalRayPoint = finalHit.point;

            localRayPoint.y += hoverRayDistance;
            finalRayPoint.y += hoverRayDistance;

            target.bodyTarget.position = localRayPoint;
            target.finalTarget.position = finalRayPoint;
        }
    }

    private void MoveLegs()
    {
        foreach (var target in targets)
        {
            if (TargetsDistance(target, false) < maxSqrTargetDistance && !target.IsMoving) continue;
            if (target.isEven && oddIsWalking || !target.isEven && evenIsWalking) continue;

            if (!target.IsMoving)
            {
                if (target.isEven)
                    this.evenIsWalking = true;

                else
                    this.oddIsWalking = true;

                lerpLeg = 0;
            }

            target.SetIsMoving(true);
            float height = legArcHeight;
            float arc = Mathf.Sin(lerpLeg * Mathf.PI) * height;
            lerpLeg += Time.deltaTime * legArcSpeed;

            var newPosition = Vector3.Lerp(target.effectorTarget.position, target.finalTarget.position, legStepSpeed * Time.deltaTime);

            if (lerpLeg < 1)
                newPosition.y += arc;

            else
                newPosition = target.finalTarget.position;

            target.effectorTarget.position = newPosition;
            target.IncrementStepTime(Time.deltaTime);

            if (TargetsDistance(target) <= 0.001f)
            {
                target.SetIsMoving(false);
                target.ResetStepTime();
                target.effectorTarget.position = target.finalTarget.position;

                if (this.evenIsWalking)
                    this.evenIsWalking = false;

                else
                    this.oddIsWalking = false;
            }
        }
    }

    private void MoveFinalTargets()
    {
        var offset = xFinalTargetOffest;

        // body is moving left
        if (previousXBodyPosition > body.position.x)
            offset *= -1;

        if (previousXBodyPosition == body.position.x)
            return;

        foreach (var target in targets)
        {
            var pos = target.bodyTarget.position;
            target.finalTarget.position = new Vector3(pos.x + offset, target.bodyTarget.position.y, pos.z);
        }

        previousXBodyPosition = body.position.x;
    }

    private void CalculateBodyPosition()
    {
        Vector3 meanPosition = GetMeanLegsPosition();

        body.position = new Vector3(body.position.x, meanPosition.y + bodyPositionOffset, body.position.z);
    }

    private void CalculateBodyRotation()
    {
        Vector3 meanDirection = GetMeanLegsDirection();
        float angle = (Mathf.Atan2(meanDirection.y, meanDirection.x) * Mathf.Rad2Deg) - 90;
        body.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    #endregion

    #region Getters Aux
    private float TargetsDistance(ObjectTargets objectTarget, bool byFinalTarget = true)
    {
        if (byFinalTarget)
            return (objectTarget.finalTarget.position - objectTarget.effectorTarget.position).sqrMagnitude;

        else
            return (objectTarget.bodyTarget.position - objectTarget.effectorTarget.position).sqrMagnitude;
    }

    private Vector3 GetMeanLegsPosition()
    {
        float x = 0f, y = 0f, z = 0f;
        int numberOfPositions = targets.Count;

        foreach (ObjectTargets target in targets)
        {
            x += target.effectorTarget.position.x;
            y += target.effectorTarget.position.y;
            z += target.effectorTarget.position.z;
        }

        return new Vector3(x / numberOfPositions, y / numberOfPositions, z / numberOfPositions);
    }

    private Vector3 GetMeanLegsDirection()
    {
        Vector3 centerPoint = GetMeanLegsPosition();
        Vector3 legPoint = targets[1].effectorTarget.position;
        Vector3 legVector = legPoint - centerPoint;

        Debug.DrawRay(centerPoint, new Vector3(-legVector.y, legVector.x, 0).normalized, Color.red);
        return new Vector3(-legVector.y, legVector.x, 0).normalized;
    }
    #endregion

    #endregion
}
