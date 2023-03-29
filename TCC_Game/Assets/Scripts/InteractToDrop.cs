using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractToDrop : MonoBehaviour, IInteractable
{
    [SerializeField] IsDamagedAndDead life;
    public void Interact() 
    {
        life.Drop();
    }
}
