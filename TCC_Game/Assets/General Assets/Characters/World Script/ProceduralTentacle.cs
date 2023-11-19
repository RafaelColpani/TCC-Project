using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KevinCastejon.MoreAttributes;
using System;

[System.Serializable]
public class Tentacle
{
    [Tooltip("The transform of the target of the IK")]
    public Transform target;
    [Tooltip("The positions that the target will move in idle animation")]
    public Vector3[] targetsIdleAnimation;

    [HideInInspector] public bool[] movingToIdle;
    [HideInInspector] public bool idleIsMovingForward;
}

public class ProceduralTentacle : MonoBehaviour
{
    enum TentacleState
    {
        IDLE, FOLLOWING
    }

    #region Inspector Vars
    [HeaderPlus(" ", "- TARGETS -", (int)HeaderPlusColor.green)]
    [Tooltip("The list of targets and its idle animation positions")]
    [SerializeField] Tentacle[] tentacles;
    [Tooltip("The list of the object TAGs that the tentacle will try to reach in the follow state of the animation. Basically, the objects that" +
        "will trigger the tentacle follow when entered the collider.")]
    [SerializeField] string[] objectToFollowTags;

    [HeaderPlus(" ", "- IDLE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The speed that the target will move to the positions of the idle animation")]
    [SerializeField] private float idleAnimationSpeed;
    [Tooltip("Tells the distance that the target have to be from idle anim destination to change its destination")]
    [SerializeField] private float idleDistance = 0.2f;

    [HeaderPlus(" ", "- FOLLOW -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The speed that the target will move to the position of the object to follow")]
    [SerializeField] private float followSpeed;
    #endregion

    #region VARs
    private TentacleState state = TentacleState.IDLE;

    private List<Transform> objectsInRange = new List<Transform>();

    private bool proceduralIsOn = true;
    #endregion

    #region Public VARS
    public bool ProceduralIsOn
    {
        get { return proceduralIsOn; }
        set { proceduralIsOn = value; }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        for (int i = 0; i < tentacles.Length; i++)
            tentacles[i].idleIsMovingForward = true;

        ResetIdleAnimationPosition();
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;
        if (!proceduralIsOn) return;

        switch (state)
        {
            case TentacleState.IDLE:
                IdleAnimation();
                break;

            case TentacleState.FOLLOWING:
                FollowAnimation();
                break;
        }
    }
    #endregion

    #region Private Methods
    #region IDLE
    private void IdleAnimation()
    {
        foreach (var tentacle in tentacles)
        {
            var goToPositionIndex = Array.IndexOf(tentacle.movingToIdle, true);
            if (goToPositionIndex == -1) { Debug.LogError($"GOT INDEX -1 IN TENTACLE IDLE ANIMATION OF {this.gameObject.name}"); return; }

            var targetsDistance = (tentacle.target.localPosition - tentacle.targetsIdleAnimation[goToPositionIndex]).sqrMagnitude;
            var time = idleAnimationSpeed;
            MoveTarget(tentacle.target, tentacle.targetsIdleAnimation[goToPositionIndex], time);
            if (targetsDistance < idleDistance)
            {
                SetNewIdleAnimationPosition(tentacle);
            }
        }
    }

    private void ResetIdleAnimationPosition()
    {
        for (int i = 0; i < tentacles.Length; i++)
        {
            bool[] aux = new bool[tentacles[i].targetsIdleAnimation.Length];
            for (var j = 0; j < aux.Length; j++)
            {
                if (j == aux.Length - 1)
                    { aux[j] = true; break; }

                else
                    aux[j] = false;
            }

            tentacles[i].movingToIdle = aux;
        }
    }

    private void SetNewIdleAnimationPosition(Tentacle tentacle)
    {
        for (var i = 0; i < tentacle.movingToIdle.Length; i++)
        {
            if (!tentacle.movingToIdle[i]) continue;

            if (i == tentacle.movingToIdle.Length - 1)
                tentacle.idleIsMovingForward = false;

            else if (i == 0)
                tentacle.idleIsMovingForward = true;

            SwapIdleAnimationPosition(tentacle, i);

            break;
        }
    }

    private void SwapIdleAnimationPosition(Tentacle tentacle, int index)
    {
        tentacle.movingToIdle[index] = false;

        if (tentacle.idleIsMovingForward)
            tentacle.movingToIdle[index + 1] = true;

        else
             tentacle.movingToIdle[index - 1] = true;
    }
    #endregion

    #region FOLLOW
    private void FollowAnimation()
    {
        Transform closerObject = this.transform;
        float closerDistance = Mathf.Infinity;

        foreach (var objectInRange in objectsInRange)
        {
            var distance = (objectInRange.position - this.transform.position).sqrMagnitude;

            if (distance >= closerDistance) continue;

            closerDistance = distance;
            closerObject = objectInRange;
        }

        if (closerObject == this.transform) return;

        foreach (var tentacle in tentacles)
            MoveTarget(tentacle.target, closerObject.position, followSpeed, true);
    }
    #endregion

    #region AUX
    private void MoveTarget(Transform target, Vector3 finalPosition, float speed, bool invertPoint = false)
    {
        var relativePosition = Vector3.zero;

        if (invertPoint)
            relativePosition = transform.InverseTransformPoint(finalPosition);

        var newPosition = Vector3.Slerp(target.localPosition,
                                            invertPoint ? relativePosition : finalPosition,
                                            speed * Time.fixedDeltaTime);

        target.localPosition = newPosition;
    }

    /// <summary>Verify if the object in collider is inside the list of tag detection</summary>
    /// <param name="collision"></param>
    private bool IsRightTag(Collider2D collision)
    {
        for (int i = 0; i < objectToFollowTags.Length; i++)
        {
            if (collision.CompareTag(objectToFollowTags[i]))
                break;

            if (i == objectToFollowTags.Length - 1)
                return false;
        }

        return true;
    }
    #endregion
    #endregion

    #region Unity Events
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsRightTag(collision)) return;

        // adds the transform object to the list of objects if is not already at the list
        if (!objectsInRange.Contains(collision.transform))
            objectsInRange.Add(collision.transform);

        if (state != TentacleState.FOLLOWING)
            state = TentacleState.FOLLOWING;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsRightTag(collision)) return;

        // removes the transform object of the list of objects if is not already removed
        if (objectsInRange.Contains(collision.transform))
            objectsInRange.Remove(collision.transform);

        if (objectsInRange.Count() <= 0)
            state = TentacleState.IDLE;
    }
    #endregion
}
