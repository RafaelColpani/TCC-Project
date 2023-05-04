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
    private bool isInSlope;

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
        this.isInSlope = false;
    }

    public void Execute(Transform actor, CharacterController characterController = null, float value = 1)
    {
        if (!isInSlope)
            velocity = actor.right;

        ChangeDirection(actor, value);

        if (!canWalk) return;
        if (!canMove) return;

        if (characterController != null)
        {
            characterController.Move((velocity * (value * walkSpeed)) * Time.fixedDeltaTime);
            Debug.Log(velocity);
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
            this.isInSlope = false;
            return; 
        }

        this.isInSlope = true;
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
    public static bool IsGrounded(Transform groundCheck, float groundCheckRadius, LayerMask groundLayer)
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundCheck.position, groundCheckRadius, Vector2.zero, Mathf.Infinity, groundLayer);
        return hit.collider != null;
    }
}
#endregion
