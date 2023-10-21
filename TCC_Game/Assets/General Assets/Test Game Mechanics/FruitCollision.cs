using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("colidi com: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            print("colidi com o preie");

            FruitHorizontalCreator fhc = FindObjectOfType<FruitHorizontalCreator>();

            transform.position = fhc.finalDestination.transform.position;

            fhc.CallAnimator();

        Destroy(gameObject);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    
}
