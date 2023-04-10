using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;

#region COMMANDS
public class MoveCommand : ICommand
{
    private float walkSpeed;

    public MoveCommand(float walkSpeed, float gravity = 9.81f)
    {
        this.walkSpeed = walkSpeed;
    }

    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
    {
        Vector3 movement = actor.transform.right * (value * walkSpeed);

        // moving by character controller
        if (characterController != null)
        {
            characterController.Move(movement * Time.fixedDeltaTime);
        }
    }
}

public class PressJumpCommand : ICommand
{
    private float jumpForce;
    private float groundCheckRadius;
    private Transform groundCheck;
    private LayerMask groundLayer;

    public PressJumpCommand(float jumpForce, Transform groundCheck, float groundCheckRadius, LayerMask groundLayer)
    {
        this.jumpForce = jumpForce;
        this.groundCheck = groundCheck;
        this.groundCheckRadius = groundCheckRadius;
        this.groundLayer = groundLayer;
    }

    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
    {
        if (!JumpUtils.IsGrounded(groundCheck, groundCheckRadius, groundLayer)) return;
        //TODO: Jump Performed

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