using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectTargets
{
    public Transform localTarget;
    public Transform worldTarget;
    public Transform effector;
    public bool isEven;

    private float stepTime;
    private bool isMoving;
    private Vector3 moveLegPosition;

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

    [HideInInspector] public Vector3 MoveLegPosition
    {
        get { return moveLegPosition; }
        set { moveLegPosition = value; }
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

public class SpiderProcedural : MonoBehaviour
{
    [SerializeField] private List<ObjectTargets> targets;
    [SerializeField] private float hoverRayDistance;
    [SerializeField] private float maxSqrTargetDistance;
    [SerializeField] private float legStepSpeed;
    [SerializeField] private float legArcHeight;
    [SerializeField] private float legArcSpeed;

    private bool evenIsWalking = false;
    private bool oddIsWalking = false;

    private float lerpLeg = 0;

    private void Start()
    {
        foreach (var target in targets)
        {
            target.ResetStepTime();
            target.SetIsMoving(false);
        }
    }

    private void LateUpdate()
    {
        TargetsGroundHeight();
        MoveLegs();
    }

    private void TargetsGroundHeight()
    {
        RaycastHit2D hit;

        foreach (var target in targets)
        {
            hit = Physics2D.Raycast(target.localTarget.position, -Vector2.up);

            if (hit.collider == null)
                continue;

            Vector3 rayPoint = hit.point;
            rayPoint.y += hoverRayDistance;
            target.localTarget.position = rayPoint;
        }
    }

    private float TargetsDistance(ObjectTargets objectTarget, bool byRealTimePosition = true)
    {
        if (byRealTimePosition)
            return (objectTarget.localTarget.position - objectTarget.worldTarget.position).sqrMagnitude;

        else
            return (objectTarget.MoveLegPosition - objectTarget.worldTarget.position).sqrMagnitude;
    }

    private void MoveLegs()
    {
        foreach (var target in targets)
        {
            if (TargetsDistance(target) < maxSqrTargetDistance && !target.IsMoving) continue;
            if (target.isEven && oddIsWalking || !target.isEven && evenIsWalking) continue;

            if (!target.IsMoving)
            {
                target.MoveLegPosition = target.localTarget.position;

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

            var newPosition = Vector3.Lerp(target.worldTarget.position, target.localTarget.position, legStepSpeed * Time.deltaTime);

            if (lerpLeg < 1)
                newPosition.y += arc;

            else
                newPosition = target.localTarget.position;

            target.worldTarget.position = newPosition;
            target.IncrementStepTime(Time.deltaTime);

            if (TargetsDistance(target) <= 0.001f) //
            {
                target.SetIsMoving(false);
                target.ResetStepTime();
                //target.worldTarget.position = target.localTarget.position;

                if (this.evenIsWalking)
                    this.evenIsWalking = false;

                else
                    this.oddIsWalking = false;
            }
        }
    }
}
