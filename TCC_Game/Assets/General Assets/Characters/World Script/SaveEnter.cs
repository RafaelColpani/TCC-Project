using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEnter : MonoBehaviour, IInteractable
{
    [SerializeField] float waitBetweenChangeAndSave = 1;
    [SerializeField] float waitForRune = 0.5f;
    Collider2D collider;
    bool firstTime = true;
    //SpriteRenderer sprRender;

    private void Awake()
    {
        collider = this.GetComponent<Collider2D>();
        // sprRender = GetComponent<SpriteRenderer>();
    }
    public void Interact()
    {
        if (collider.enabled)
        {
            collider.enabled = false;

            if (firstTime)
            {
                StartCoroutine(ChangeTree());
            }
            else
            {
                StartCoroutine(Save());
            }
        }
    }

    IEnumerator ChangeTree()
    {
        print("First time in save" + "changed" + "[ SaveEnter.cs ]");
        firstTime = false;
        //sprRender.color = Color.red;
        yield return new WaitForSeconds(waitBetweenChangeAndSave);
        StartCoroutine(Save());
    }

    IEnumerator Save()
    {
        print("Save DONE and disabled! Run animation" + " " + "[ SaveEnter.cs ]");
        //print("Boom");
        yield return new WaitForSeconds(waitForRune);
        //print("show rune");
    }
}
