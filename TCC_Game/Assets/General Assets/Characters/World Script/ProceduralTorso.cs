using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using KevinCastejon.MoreAttributes;
using System.Linq;

public class ProceduralTorso : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- BONES -", (int)HeaderPlusColor.green)]
    [Tooltip("The target that control de IK of the torso")]
    [SerializeField] private Transform target;
    [Tooltip("The Transform where is located the bone of the head of the character")]
    [SerializeField] private Transform headBone;

    [HeaderPlus(" ", "- WALK POSITIONS -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The local position that the target will be when walking")]
    [SerializeField] private Vector3 walkingTargetLocalPosition;
    [Tooltip("The z rotation value that the head will perform while walking")]
    [SerializeField] private float headRotationValue;
    #endregion

    #region Unity Methods
    private void FixedUpdate()
    {
        if (PauseController.GetIsPaused()) return;


    }
    #endregion

    #region Private Methods
    #endregion
}
