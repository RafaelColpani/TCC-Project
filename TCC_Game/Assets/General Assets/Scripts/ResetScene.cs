using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqui, você pode reiniciar a cena atual.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
