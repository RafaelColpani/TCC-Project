using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private float bodyPositionOffset;
    [SerializeField] private Transform body;

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
        CalculateBodyPosition();
        CalculateBodyRotation();
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

    private void CalculateBodyPosition()
    {
        Vector3 meanPosition = GetMeanLegsPosition();

        body.position = new Vector3(body.position.x, meanPosition.y * bodyPositionOffset, body.position.z);
    }

    private Vector3 GetMeanLegsPosition()
    {
        float x = 0f, y = 0f, z = 0f;
        int numberOfPositions = targets.Count;

        foreach (ObjectTargets target in targets)
        {
            x += target.worldTarget.position.x;
            y += target.worldTarget.position.y;
            z += target.worldTarget.position.z;
        }

        return new Vector3(x / numberOfPositions, y / numberOfPositions, z / numberOfPositions);
    }

    private void CalculateBodyRotation()
    {
        Vector3 meanDirection = GetMeanLegsDirection();
        float angle = Mathf.Atan2(meanDirection.y, meanDirection.x) * Mathf.Rad2Deg;
        body.transform.rotation = Quaternion.LookRotation(meanDirection);
    }

    private Vector3 GetMeanLegsDirection()
    {
        Vector3 centerPoint = GetMeanLegsPosition();
        Vector3 legPoint = targets[1].worldTarget.position;
        Vector3 legVector = legPoint - centerPoint;
        Debug.DrawRay(centerPoint, new Vector3(-legVector.y, legVector.x, 0).normalized, Color.red);
        print(new Vector3(-legVector.y, legVector.x, 0).normalized);

        return new Vector3(-legVector.y, legVector.x, 0).normalized;
    }
}
