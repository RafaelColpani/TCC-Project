using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    IInteractable interactable;

    private void OnTriggerStay2D(Collider2D collision)
    {
        var mb = collision.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour mono in mb) 
        {
            if (mono is IInteractable)
            {
                interactable = mono as IInteractable;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var mb = collision.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour mono in mb)
        {
            if (mono is IInteractable)
            {
                if(interactable == mono as IInteractable)
                interactable = null;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && interactable != null)
        {
            interactable.Interact();
        }
    }
}
