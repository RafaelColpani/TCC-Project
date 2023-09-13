using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
using KevinCastejon.MoreAttributes;
using System.Linq;

[System.Serializable]
public class ArmsTargets
{
    [Tooltip("The target that he arm effector will follow")]
    public Transform target;
    [Tooltip("The effector that will link the bones")]
    public Transform effector;
    [Tooltip("The height that the target will be, based on body position")]
    public float targetHeightOffset;
    [Tooltip("Each arm will move in a different direction, problably accordingly to the oposite leg, so it's " +
        "recommended to mark this as oposite to the same side leg (Example: if right leg is true, the right arm must be false)")]
    public bool isEven;
}

public class ProceduralArms : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- ARMS TARGETS -", (int)HeaderPlusColor.green)]
    [SerializeField] private List<ArmsTargets> armsTargets;
    #endregion

    #region Private VARs
    private CharacterManager characterManager;

    private Transform body;
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterManager = GetComponent<CharacterManager>();

        body = characterManager.Body;
    }

    private void FixedUpdate()
    {
        CalculateTargetsHeight();
    }
    #endregion

    #region Private Methods
    private void CalculateTargetsHeight()
    {
        var bodyPosition = body.position;

        foreach(var armTarget in armsTargets)
        {
            armTarget.target.localPosition = new Vector3(armTarget.target.localPosition.x, armTarget.targetHeightOffset, armTarget.target.position.z);
        }
    }
    #endregion
}
