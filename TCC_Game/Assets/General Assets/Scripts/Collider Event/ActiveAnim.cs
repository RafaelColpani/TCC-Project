using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnim : MonoBehaviour
{
    #region Inspector Vars
    [Tooltip("The sscript that will be activated when executed the interaction.")]
    [SerializeField] MonoBehaviour scriptParaAtivar;
    [Tooltip("The time that will permit a reactivation.")]
    [SerializeField] float waitReactivationTime = 1f;
    #endregion

    #region Private Vars
    private float reactivationTimer;
    #endregion

    #region Unity Methods
    private void Start()
    {
        reactivationTimer = waitReactivationTime;
    }

    private void Update()
    {
        if (reactivationTimer >= waitReactivationTime) return;

        ReactivationTime();
    }
    #endregion

    #region Public Methods
    public void ActivatedTotem()
    {
        if (reactivationTimer < waitReactivationTime) return;

        reactivationTimer = 0;
        scriptParaAtivar.enabled = true;
    }
    #endregion

    #region Private Methods
    private void ReactivationTime()
    {
        reactivationTimer += Time.deltaTime;
    }
    #endregion
}
