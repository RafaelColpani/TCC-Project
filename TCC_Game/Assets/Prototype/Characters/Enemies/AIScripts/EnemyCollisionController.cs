using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionController : MonoBehaviour
{
    private bool touchedPlayer = false;

    public bool TouchedPlayer
    {
        get { return touchedPlayer; }
    }

    public void OffTouchedPlayer()
    {
        touchedPlayer = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PauseController.GetIsPaused()) return;

        if (collision.CompareTag("Player"))
        {
            touchedPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchedPlayer = false;
        }
    }
}
