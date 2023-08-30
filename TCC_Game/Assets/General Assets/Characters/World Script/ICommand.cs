using UnityEngine;

interface ICommand
{
    /// <summary>Executes the command given the specific actor.</summary>
    /// <param name="actor"></param>
    /// <param name="characterController"></param>
    /// <param name="value"></param>
    public void Execute(Transform actor, float value = 1, CharacterController characterController = null);
}
