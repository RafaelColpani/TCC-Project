using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoSecondFinal : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene("SecondaryFinal");
            }
        }
    }

   
}
