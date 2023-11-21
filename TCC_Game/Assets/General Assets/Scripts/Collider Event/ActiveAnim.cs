using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnim : MonoBehaviour
{
    #region Inspector Vars
    [Tooltip("The sscript that will be activated when executed the interaction.")]
    [SerializeField] MonoBehaviour scriptParaAtivar;
    #endregion

    #region Public Methods
    public void ActivatedTotem()
    {
        if (scriptParaAtivar.isActiveAndEnabled) return;

        scriptParaAtivar.enabled = true;
    }
    #endregion
}
