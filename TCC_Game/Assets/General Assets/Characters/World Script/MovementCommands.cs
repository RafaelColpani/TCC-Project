using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Interactions;

#region COMMANDS
/// <summary>Perform the walk movement, given the actors Transform who will execute it.</summary>
public class MoveCommand : ICommand
{
    private ProceduralLegs proceduralLegs;

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

    public float CurrentSpeed = 0f;

    public MoveCommand(ProceduralLegs proceduralLegs, float walkSpeed)
    {
        this.proceduralLegs = proceduralLegs;
        this.walkSpeed = walkSpeed;
        this.velocity = Vector3.zero;
        this.isFacingRight = true;
        this.canWalk = true;
        this.canMove = true;
        this.isOnSlope = false;
    }

    public void Execute(Transform actor, float value = 1, CharacterController characterController = null)
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

        else
        {
            actor.Translate(velocity * (value * walkSpeed) * Time.fixedDeltaTime, Space.World);
        }

        this.CurrentSpeed = walkSpeed * value;
    }

    #region Metohds
    private void ChangeDirection(Transform actor, float value)
    {
        bool proceduralWasOff = false;

        if ((isFacingRight && value < 0) || (!isFacingRight && value > 0))
        {
            if (proceduralLegs.ProceduralIsOn)
            {
                proceduralWasOff = true;
                proceduralLegs.ProceduralIsOn = false;
            }

            isFacingRight = !isFacingRight;
            actor.localScale = new Vector3(actor.localScale.x * -1, actor.localScale.y, actor.localScale.z);

            if (proceduralWasOff) { }
            proceduralLegs.ProceduralIsOn = true;
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
    #endregion

    #region Getters
    public float GetXVelocity()
    {
        return this.velocity.x;
    }

    public float GetYVelocity()
    {
        return this.velocity.y;
    }

    public void SetWalkSpeed(float value)
    {
        this.walkSpeed = value;
    }
    #endregion
}

/// <summary>Perform the jump movement, given the actors Transform and its GravityController who will execute it</summary>
public class PressJumpCommand : ICommand
{
    private float jumpForce;
    private float groundCheckRadius;
    private Transform[] groundChecks;
    private LayerMask groundLayer;
    private GravityController gc;

    private bool canJump;

    public PressJumpCommand(float jumpForce, Transform[] groundChecks, float groundCheckRadius, LayerMask groundLayer, GravityController gc)
    {
        this.jumpForce = jumpForce;
        this.groundChecks = groundChecks;
        this.groundCheckRadius = groundCheckRadius;
        this.groundLayer = groundLayer;
        this.gc = gc;
        this.canJump = true;
    }

    public void Execute(Transform actor, float value = 1, CharacterController characterController = null)
    {
        if (!JumpUtils.IsGrounded(groundChecks, groundCheckRadius, groundLayer)) return;
        if (!canJump) return;

        gc.Velocity = new Vector3(gc.Velocity.x, jumpForce, gc.Velocity.z);
    }

    public void SetCanJump(bool value = true)
    {
        this.canJump = value;
    }
}

/// <summary>Perform the released jump button movement, given the actors Transform and its GravityController who will execute it</summary>
public class ReleaseJumpCommand : ICommand
{
    public void Execute(Transform actor, float value = 1, CharacterController characterController = null)
    {
        //TODO: Jump Released
        Debug.Log($"Released Jump");
    }
}

/// <summary>Perform the released shoot projectile movement, given the PlayerShooter script.</summary>
public class ShootCommand : ICommand
{
    private PlayerShooter playerShooter;

    public ShootCommand(PlayerShooter playerShooter)
    {
        this.playerShooter = playerShooter;
    }

    public void Execute(Transform actor, float value = 1, CharacterController characterController = null)
    {
        Debug.Log($"{playerShooter.gameObject.name}");
        this.playerShooter.ExecuteShootCommand();
    }
}
#endregion

#region UTILS
public class JumpUtils
{
    public static bool IsGrounded(Transform[] groundChecks, float groundCheckDistance, LayerMask groundLayer)
    {
        foreach (var groundCheck in groundChecks)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

            if (hit.collider != null && !hit.collider.isTrigger)
                return true;
        }

        return false;
    }

    public static bool UniqueGroundCheckIsGrounded(Transform groundCheck, float groundCheckDistance, LayerMask groundLayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (hit.collider != null && !hit.collider.isTrigger)
            return true;

        return false;
    }

    public static bool MoreThenHalfLegsIsGrounded(Transform[] groundChecks, float groundCheckDistance, LayerMask groundLayer)
    {
        int legsCount = groundChecks.Length;

        int groundedLegsCount = 0;

        foreach (var groundCheck in groundChecks)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

            if (hit.collider != null && !hit.collider.isTrigger)
                groundedLegsCount++;
        }

        return groundedLegsCount > legsCount / 2;
    }
}
#endregion
