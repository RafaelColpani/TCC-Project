using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GoSecondFinal : MonoBehaviour
{
    private PlayerActions playerActions;
    private bool isFire = false;

    void Awake() 
    {
        playerActions = new PlayerActions();

        playerActions.Movement.SecondFinal.performed += _ => GoSecondFinall();
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isFire = false;
        }
    }

    void GoSecondFinall() 
    {
        if (isFire == true)
        {
            SceneManager.LoadScene("SecondaryFinal");
        }
    }

    #region Enable & Disable
    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
    #endregion
}
