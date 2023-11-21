using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnim : MonoBehaviour
{
    [Header("Obj Anim")]
    [SerializeField]  Animator[] animator; 
    [Space(5)]

    [Header("Obj Activate")]
    [SerializeField]  GameObject[] objectToActivate; 
    private bool hasActivated = false; 


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            hasActivated = true; 

            if (animator != null)
            {
                foreach(Animator anim in animator){
                anim.enabled = true;
                }
            }

            if (objectToActivate != null)
            {
                foreach(GameObject obj in objectToActivate)
                {
                  obj.SetActive(true);
                }
            }
        }
    }
}
