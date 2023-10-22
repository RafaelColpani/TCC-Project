using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : MonoBehaviour
{
    [Header("Player Tag")]
    [SerializeField] string playerTag = "Player";

    [Header("Obj to Destroy")]
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] GameObject vfxToDestroy;

    [Header("Obj to Activate")]
    [SerializeField]  bool _ActiveOnOff;
    [SerializeField]  GameObject objectToActivate; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {

            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
                Destroy(vfxToDestroy);
            }

            if(_ActiveOnOff == true)
            {
                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                }
            }
        }
    }
}
