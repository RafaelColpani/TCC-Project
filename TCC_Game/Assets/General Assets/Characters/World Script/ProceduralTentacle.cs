using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KevinCastejon.MoreAttributes;
using System;

public class ProceduralTentacle : MonoBehaviour
{
    enum TentacleState
    {
        IDLE, FOLLOWING
    }

    #region Inspector Vars
    [HeaderPlus(" ", "- TARGETS -", (int)HeaderPlusColor.green)]
    [Tooltip("The transform of the target of the IK")]
    [SerializeField] Transform target;
    [Tooltip("The list of the object TAGs that the tentacle will try to reach in the follow state of the animation. Basically, the objects that" +
        "will trigger the tentacle follow when entered the collider.")]
    [SerializeField] string[] objectToFollowTags;

    [HeaderPlus(" ", "- IDLE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The positions that the target will move in idle animation")]
    [SerializeField] private Vector3[] targetsIdleAnimation;
    [Tooltip("The speed that the target will move to the positions of the idle animation")]
    [SerializeField] private float idleAnimationSpeed;

    [HeaderPlus(" ", "- FOLLOW -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The speed that the target will move to the position of the object to follow")]
    [SerializeField] private float followSpeed;
    #endregion

    #region VARs
    private TentacleState state = TentacleState.IDLE;

    private List<Transform> objectsInRange = new List<Transform>();

    private float defaultXTargetPosition;

    private bool[] movingToIdle;
    private bool idleIsMovingForward = true;
    #endregion

    #region Unity Methods
    private void Start()
    {
        defaultXTargetPosition = target.position.x;
        ResetIdleAnimationPosition();
    }

    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;

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
        var goToPositionIndex = Array.IndexOf(movingToIdle, true);
        if (goToPositionIndex == -1) { Debug.LogError($"GOT INDEX -1 IN TENTACLE IDLE ANIMATION OF {this.gameObject.name}"); return; }

        var targetsDistance = (target.localPosition - targetsIdleAnimation[goToPositionIndex]).sqrMagnitude;
        var time = idleAnimationSpeed;
        MoveTarget(targetsIdleAnimation[goToPositionIndex], time);
        if (targetsDistance < 0.2f)
        {
            SetNewIdleAnimationPosition();
        }
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
        MoveTarget(closerObject.position, followSpeed, true);
    }
    #endregion

    #region AUX
    private void MoveTarget(Vector3 finalPosition, float speed, bool invertPoint = false)
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