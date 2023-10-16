using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Test"))
        {
            // Destrua o objeto atual quando a colisão com a tag "Test" ocorrer.
            Destroy(gameObject);
        }

        if (other.CompareTag("Bullet"))
        {
            // Destrua o objeto atual quando a colisão com a tag "Test" ocorrer.
            Destroy(gameObject);
        }
    }
}
