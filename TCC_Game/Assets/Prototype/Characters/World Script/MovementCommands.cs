using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#region COMMANDS
/// <summary>Perform the walk movement, given the actors Transform who will execute it.</summary>
public class MoveCommand : ICommand
{
    private float walkSpeed;
    private Vector3 moveDirection;
    private bool isFacingRight;
    private bool canMove;

    public bool CanMove
    {
        get { return this.canMove; }
        set { this.canMove = value; }
    }

    public MoveCommand(float walkSpeed)
    {
        this.walkSpeed = walkSpeed;
        this.moveDirection = Vector3.zero;
        this.isFacingRight = true;
        this.canMove = true;
    }

    public void Execute(Transform actor, CharacterController characterController = null, float value = 1)
    {
        Vector3 movement = actor.right * (value * walkSpeed);
        ChangeDirection(actor, movement);

        // moving by character controller
        if (!canMove) return;

        if (characterController != null)
        {
            characterController.Move(movement * Time.fixedDeltaTime);
        }
    }

    public void ChangeMoveDirection(Vector3 value)
    {
        // rotate 90 degress (counter clock-wise), looking to the left
        this.moveDirection = new Vector3(-value.y, value.x, 0);
        this.moveDirection *= -1;

        //inverts if is looking to the right
        //if (isFacingRight)
            //this.moveDirection *= -1;
    }

    private void ChangeDirection(Transform actor, Vector3 movement)
    {
        if ((isFacingRight && movement.x < 0) || (!isFacingRight && movement.x > 0))
        {
            isFacingRight = !isFacingRight;
            actor.localScale = new Vector3(actor.localScale.x * -1, actor.localScale.y, actor.localScale.z);
        }
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