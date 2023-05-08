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
    [Tooltip("The transform in the position that will check if the object is grounded.")]
    [SerializeField] private Transform groundCheck;
    [Tooltip("The radius of the circle that will detect the ground from the checkGround Transform position.")]
    [SerializeField] private float groundCheckRadius;

    [HeaderPlus(" ", "- Ground -", (int)HeaderPlusColor.cyan)]
    [Tooltip("Tells if this character moves with Input Handler (player).")]
    [SerializeField] private bool commandsByInputHandler;
    #endregion

    #region Getters
    // BODY
    public Transform Body { get { return body; } }

    // GROUND
    public LayerMask GroundLayers { get { return groundLayers; } }
    public Transform GroundCheck { get { return groundCheck; } }
    public float GroundCheckRadius { get { return groundCheckRadius; } }

    // INPUT
    public bool CommandsByInputHandler { get { return commandsByInputHandler; } }
    #endregion
}
