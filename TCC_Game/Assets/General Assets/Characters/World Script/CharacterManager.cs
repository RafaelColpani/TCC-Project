using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class CharacterManager : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- BODY -", (int)HeaderPlusColor.green)]
    [Tooltip("The object that parents all bones objects. Must be an empty object holding the bones objects as a parent.")]
    [SerializeField] private Transform body;

    [HeaderPlus(" ", "- Ground -", (int)HeaderPlusColor.yellow)]
    [Tooltip("Says what layers the targets will raycast to.")]
    [SerializeField] private LayerMask groundLayers;
    [Tooltip("The the Transforms that will be used as ground checks (preferred to be the legs effector).")]
    [SerializeField] private Transform[] groundChecks;
    [Tooltip("The transform ground checks from the left part of the character.")]
    [SerializeField] private float groundCheckDistance;

    [HeaderPlus(" ", "- Ground -", (int)HeaderPlusColor.cyan)]
    [Tooltip("Tells if this character moves with Input Handler (player).")]
    [SerializeField] private bool commandsByInputHandler;
    #endregion

    #region Getters
    // BODY
    public Transform Body { get { return body; } }

    // GROUND
    public LayerMask GroundLayers { get { return groundLayers; } }
    public Transform[] GroundChecks { get { return groundChecks; } }
    public float GroundCheckDistance { get { return groundCheckDistance; } }

    // INPUT
    public bool CommandsByInputHandler { get { return commandsByInputHandler; } }
    #endregion

    #region Public Methods
    public int DirectionMultiplier()
    {
        if (IsFacingRight())
            return 1;

        else
            return -1;
    }

    public bool IsFacingRight()
    {
        return this.transform.localScale.x > 0;
    }
    #endregion
}
