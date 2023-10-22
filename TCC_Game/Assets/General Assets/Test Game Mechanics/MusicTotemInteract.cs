using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTotemInteract : MonoBehaviour, IInteractable
{
    [SerializeField] MusicPuzzle musicPuzzle;

    private bool isInteracted = false;

    public bool IsInteracted
    {
        get { return isInteracted; }
    }

    public void Interact()
    {
        if (isInteracted) return;

        isInteracted = true;
        musicPuzzle.TotemInteracted(this.gameObject);
    }

    public void ResetInteracted()
    {
        isInteracted = false;
    }
}
