using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] Collider2D saveCol;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player")) 
        {
            print("save enabled");
            saveCol.enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
