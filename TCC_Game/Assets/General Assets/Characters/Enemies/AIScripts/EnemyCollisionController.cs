using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionController : MonoBehaviour
{
    #region Inspector
    [Tooltip("The collider that the enemy will only patrol inside.")]
    [SerializeField] private Collider2D regionCollider;
    #endregion

    #region VARs
    private bool touchedPlayer = false;
    private bool exitPatrolRegion = false;
    #endregion

    #region Getters & Setters
    public bool TouchedPlayer
    {
        get { return touchedPlayer; }
    }

    public bool ExitPatrolRegion
    {
        get { return exitPatrolRegion; }
    }
    #endregion

    #region Public Methods
    public void OffTouchedPlayer()
    {
        touchedPlayer = false;
    }

    public void OffExitPatrolRegion()
    {
        exitPatrolRegion = false;
    }
    #endregion

    #region Collision Events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == this.regionCollider)
        {
            exitPatrolRegion = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PauseController.GetIsPaused()) return;

        if (collision.CompareTag("Player"))
        {
            touchedPlayer = true;
        }

        if (collision == this.regionCollider)
        {
            exitPatrolRegion = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // exit from player
        if (collision.CompareTag("Player"))
        {
            touchedPlayer = false;
        }

        // exit from patrol region
        if (collision == this.regionCollider)
        {
            exitPatrolRegion = true;
        }
    }
    #endregion
}
