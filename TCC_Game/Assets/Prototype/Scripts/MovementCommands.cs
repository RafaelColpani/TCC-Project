using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
    private InputHandler ih;
    private Rigidbody2D rb;
    private float jumpTime;
    private GravityController gc;

    private float jumpStartTime;
    private Vector3 initialPosition;

    private bool startedJump = false;
    private float velocity = 0f;

    public PressJumpCommand(float jumpForce, Transform groundCheck, float groundCheckRadius, LayerMask groundLayer, InputHandler ih, Rigidbody2D rb, float jumpTime, GravityController gc)
    {
        this.jumpForce = jumpForce;
        this.groundCheck = groundCheck;
        this.groundCheckRadius = groundCheckRadius;
        this.groundLayer = groundLayer;
        this.ih = ih;
        this.rb = rb;
        this.jumpTime = jumpTime;
        this.gc = gc;
    }

    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
    {
        if (!JumpUtils.IsGrounded(groundCheck, groundCheckRadius, groundLayer))
        {
            ih.jumped = false;
            startedJump = false;
            velocity = 0;
            return;
        }
        //if (procedural != null)
        //procedural.ProceduralIsOn = false;
        if (!startedJump)
        {
            startedJump = true;
        }

        //rigidBody.velocity = new Vector2(rigidBody.velocity.x, velocity.y);
        //actor.transform.position = Vector3.Lerp(actor.transform.position, actor.transform.up * jumpForce, 2);
        //characterController.Move(new Vector3(0, jumpForce, 0));
        //StartCoroutine(JumpCoroutine(jumpForce, 5, actor.transform));
        //velocity += jumpForce;
        //actor.transform.Translate(new Vector3(0, jumpForce, 0));
        //float vPos = rb.position.y + jumpForce * Time.fixedDeltaTime;
        //rb.MovePosition(new Vector2(rb.position.x, vPos));
        gc.Velocity = new Vector3(gc.Velocity.x, gc.Velocity.y + jumpForce, gc.Velocity.z);
        
        Debug.Log("jumping");
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