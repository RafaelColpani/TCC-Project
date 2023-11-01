using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : MonoBehaviour
{
    private readonly string playerTag = "Player";

    #region Inspector Vars
    [Header("Obj to Destroy")]
    [Tooltip("The object that will be destroyed when the event occurs")]
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] GameObject vfxToDestroy;

    [Header("Obj to Activate")]
    [SerializeField]  bool _ActiveOnOff;
    [Tooltip("The object that will be activated when the event occurs")]
    [SerializeField]  GameObject objectToActivate;

    [Header("CineMachine")]
    [SerializeField] bool isCinemachineEvent;
    [SerializeField] GameObject playerVCam;
    [SerializeField] GameObject eventVCam;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] float eventTime;
    #endregion

    #region Private VARs
    private bool isActive = false;
    #endregion

    #region Getters
    public bool ActiveOnOff
    {
        get { return _ActiveOnOff; }
    }

    public bool IsActive
    {
        get { return isActive; }
    }
    #endregion

    #region Public Methods
    public void ActivatedTotem()
    {
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Destroy(vfxToDestroy);
        }

        if (_ActiveOnOff && objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        isActive = true;

        if (isCinemachineEvent)
            ActivateCinemachineEvent();
    }
    #endregion

    #region Private Methods
    private void ActivateCinemachineEvent()
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
    #endregion

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
    }
    #endregion
}
