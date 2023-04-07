using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;

public class MoveCommand : ICommand
{
    private float walkSpeed;

    public MoveCommand(float walkSpeed)
    {
        this.walkSpeed = walkSpeed;
    }

    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
    {
        // moving by character controller
        if (characterController != null)
        {
            characterController.Move(actor.transform.right * (value * walkSpeed) * Time.deltaTime);
        }
    }
}

public class PressJumpCommand : ICommand
{
    private float jumpForce;

    public PressJumpCommand(float jumpForce)
    {
        this.jumpForce = jumpForce;
    }

    public void Execute(GameObject actor, CharacterController characterController = null, float value = 1)
    {
        //TODO: Jump Pressed
        Debug.Log($"Pressed Jump");
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