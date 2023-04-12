using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#region COMMANDS
public class MoveCommand : ICommand
{
    private float walkSpeed;
    private bool isFacingRight;

    public MoveCommand(float walkSpeed, float gravity = 9.81f)
    {
        this.walkSpeed = walkSpeed;
        this.isFacingRight = true;
    }

    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
    {
        Vector3 movement = actor.transform.right * (value * walkSpeed);
        ChangeDirection(actor.transform, movement);

        // moving by character controller
        if (characterController != null)
        {
            characterController.Move(movement * Time.fixedDeltaTime);
        }
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

    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
    {
        if (!JumpUtils.IsGrounded(groundCheck, groundCheckRadius, groundLayer)) return;

        gc.Velocity = new Vector3(gc.Velocity.x, jumpForce, gc.Velocity.z);
    }
}

public class ReleaseJumpCommand : ICommand
{
    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
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