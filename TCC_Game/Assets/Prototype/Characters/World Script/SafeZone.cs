using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] Collider saveCol;
    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag.Equals("Player")) 
        {
            print("save enabled");
            saveCol.enabled = true;
        }
    }
}
