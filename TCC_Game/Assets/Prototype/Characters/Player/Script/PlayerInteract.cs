using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    IInteractable interactable;

    private void OnTriggerStay2D(Collider2D collision)
    {
        var mb = collision.GetComponents<MonoBehaviour>();
        print("Interact is start" + " " + " [PlayerInteract.cs] ");

        foreach (MonoBehaviour mono in mb)
        {
            if (mono is IInteractable)
            {
                print("entering interactable do " + collision.gameObject.name + " " + " [PlayerInteract.cs] ");
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
                if (interactable == mono as IInteractable)
                    interactable = null;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && interactable != null)
        {

            Debug.Log("Interact button" + " " + " [PlayerInteract.cs] ");
            interactable.Interact();
        }
    }
}
