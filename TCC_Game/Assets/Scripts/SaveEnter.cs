using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEnter : MonoBehaviour, IInteractable
{
    Collider2D collider;

    private void Awake()
    {
        collider = this.GetComponent<Collider2D>();
    }
    public void Interact() 
    {
        if (collider.enabled)
        {
            print("Save DONE and disabled! Run animation.");
            collider.enabled = false;
        }
    }
}
