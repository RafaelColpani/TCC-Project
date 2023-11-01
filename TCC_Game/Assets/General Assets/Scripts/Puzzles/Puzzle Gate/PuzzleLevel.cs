using System.Collections;
using System.Collections.Generic;
using KevinCastejon.MoreAttributes;
using UnityEngine;

public class PuzzleLevel : MonoBehaviour
{
    #region Inspector VARS
    [HeaderPlus(" ", "- OBJECTIVE -", (int)HeaderPlusColor.green)]
    [Tooltip("The transform of the door that will open and its BELOW the other door.")]
    [SerializeField] Transform upperDoor;
    [Tooltip("The transform of the door that will open and its ABOVE the other door.")]
    [SerializeField] Transform lowerDoor;
    [Tooltip("The position that the upper door will be after activation")]
    [SerializeField] Vector3 upperDoorPosition;
    [Tooltip("The position that the lower door will be after activation")]
    [SerializeField] Vector3 lowerDoorPosition;
    [Tooltip("The VFX that will reproduce when opened the door")]
    [SerializeField] GameObject vfxSmoke;
    #endregion

    #region Public Methods
    public void ActivatedTotem()
    {
        // moves the lower door to down
        lowerDoor.position = lowerDoorPosition; ;
        vfxSmoke.SetActive(true);

        // moves the upper door to up
        upperDoor.position = upperDoorPosition;
    }
    #endregion
}
