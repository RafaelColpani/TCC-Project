using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineTrigger : MonoBehaviour
{
    [Header("Obj to Destroy")]
    [Tooltip("The object that will be destroyed when the event occurs")]
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] GameObject vfxToDestroy;
    [SerializeField] bool destroySelf = false;

    [Header("Obj to Activate")]
    [SerializeField] bool _ActiveOnOff;
    [Tooltip("The object that will be activated when the event occurs")]
    [SerializeField] GameObject objectToActivate;

    [Header("CineMachine")]
    [SerializeField] bool isCinemachineEvent;
    [SerializeField] GameObject playerVCam;
    [SerializeField] GameObject eventVCam;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] float eventTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if(isCinemachineEvent == true) 
        {
            playerVCam.SetActive(false);
            eventVCam.SetActive(true);

            if (inputHandler != null)
            {
                inputHandler.canWalk = false;
                inputHandler.GetJumpCommand().SetCanJump(false);
            }

             StartCoroutine(DeactivateEvent());
        }

        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Destroy(vfxToDestroy);
        }

        if (_ActiveOnOff && objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }

    #region Coroutines
    private IEnumerator DeactivateEvent()
    {
        yield return new WaitForSeconds(eventTime);

        playerVCam.SetActive(true);
        eventVCam.SetActive(false);

        if (inputHandler != null)
        {
            inputHandler.canWalk = true;
            inputHandler.GetJumpCommand().SetCanJump();
        }

        if (destroySelf)
            Destroy(this.gameObject);
    }
    #endregion
}
