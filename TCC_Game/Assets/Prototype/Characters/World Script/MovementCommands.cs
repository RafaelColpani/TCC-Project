using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Interactions;

#region COMMANDS
/// <summary>Perform the walk movement, given the actors Transform who will execute it.</summary>
public class MoveCommand : ICommand
{
    private float walkSpeed;
    private Vector3 velocity;
    private bool isFacingRight;
    private bool canWalk;
    private bool canMove;
    private bool isOnSlope;

    public bool CanMove
    {
        get { return this.canMove; }
        set { this.canMove = value; }
    }

    public MoveCommand(float walkSpeed)
    {
        this.walkSpeed = walkSpeed;
        this.velocity = Vector3.zero;
        this.isFacingRight = true;
        this.canWalk = true;
        this.canMove = true;
        this.isOnSlope = false;
    }

    public void Execute(Transform actor, CharacterController characterController = null, float value = 1)
    {
        if (!isOnSlope)
            velocity = actor.right;

        ChangeDirection(actor, value);

        if (!canWalk) return;
        if (!canMove) return;

        if (characterController != null)
        {
            characterController.Move((velocity * (value * walkSpeed)) * Time.fixedDeltaTime);
        }
    }

    private void ChangeDirection(Transform actor, float value)
    {
        if ((isFacingRight && value < 0) || (!isFacingRight && value > 0))
        {
            isFacingRight = !isFacingRight;
            actor.localScale = new Vector3(actor.localScale.x * -1, actor.localScale.y, actor.localScale.z);
        }
    }

    public void ModifySlopeVelocity(bool modify = false, float xValue = 0, float yValue = 0)
    {
        if (!modify) 
        {
            this.isOnSlope = false;
            return; 
        }

        this.isOnSlope = true;
        velocity = new Vector3(xValue, yValue, 0);
    }

    public void SetCanWalk(bool value = false)
    {
        this.canWalk = value;
    }

    public float GetXVelocity()
    {
        return this.velocity.x;
    }

    public float GetYVelocity()
    {
        return this.velocity.y;
    }
}

/// <summary>Perform the jump movement, given the actors Transform and its GravityController who will execute it</summary>
public class PressJumpCommand : ICommand
{
    private float jumpForce;
    private float groundCheckRadius;
    private Transform groundCheck;
    private LayerMask groundLayer;
    private GravityController gc;


    public PressJumpCommand(float jumpForce, Transform groundCheck, float groundCheckRadius, LayerMask groundLayer, GravityController gc)
    {
        this.jumpForce = jumpForce;
        this.groundCheck = groundCheck;
        this.groundCheckRadius = groundCheckRadius;
        this.groundLayer = groundLayer;
        this.gc = gc;
    }

    public void Execute(Transform actor, CharacterController characterController = null, float value = 1)
    {
        if (!JumpUtils.IsGrounded(groundCheck, groundCheckRadius, groundLayer)) return;

        gc.Velocity = new Vector3(gc.Velocity.x, jumpForce, gc.Velocity.z);
    }
}

/// <summary>Perform the released jump button movement, given the actors Transform and its GravityController who will execute it</summary>
public class ReleaseJumpCommand : ICommand
{
    public void Execute(Transform actor, CharacterController characterController = null, float value = 1)
    {
        //TODO: Jump Released
        Debug.Log($"Released Jump");
    }
}
#endregion

#region UTILS
public class JumpUtils
{
    public static bool IsGrounded(Transform groundCheckParent, float groundCheckDistance, LayerMask groundLayer)
    {
        var groundChecks = groundCheckParent.GetComponentsInChildren<Transform>();

        foreach (var groundCheck in groundChecks)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

            if (hit.collider != null && !hit.collider.isTrigger)
                return true;
        }

        return false;
    }
}
#endregion
