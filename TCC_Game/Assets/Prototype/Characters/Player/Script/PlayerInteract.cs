using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    IInteractable interactable;

    private void OnTriggerStay(Collider collision)
    {
        var mb = collision.GetComponents<MonoBehaviour>();
        print("colliding");

        foreach (MonoBehaviour mono in mb) 
        {
            if (mono is IInteractable)
            {
                print("entering interactable do "+collision.gameObject.name);
                interactable = mono as IInteractable;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
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

            Debug.Log("F");
            interactable?.Interact();
        }
    }
}
