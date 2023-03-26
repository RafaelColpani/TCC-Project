using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractToDrop : MonoBehaviour, IInteractable
{
    [SerializeField] Life life;
    public void Interact() 
    {
        life.Drop();
    }
}
