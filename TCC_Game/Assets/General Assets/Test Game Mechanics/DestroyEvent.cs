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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Destroy(vfxToDestroy);
        }

        if(_ActiveOnOff && objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        isActive = true;
    }
}
