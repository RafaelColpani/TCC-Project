using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class MusicTotemInteract : MonoBehaviour, IInteractable
{
    #region Inspector VARs
    [HeaderPlus(" ", "- ACTIONS -", (int)HeaderPlusColor.green)]
    [Tooltip("Assign this if the totem is for music puzzle. If not, let it null")]
    [SerializeField] MusicPuzzle musicPuzzle;
    [Tooltip("Mark this if the totem is for puzzle level. If not, let it false")]
    [SerializeField] bool isPuzzleLevel = false;
    [Tooltip("Mark this if the totem is for active anim. If not, let it false")]
    [SerializeField] bool isActiveAnim = false;

    [HeaderPlus(" ", "- INTERACTION -", (int)HeaderPlusColor.yellow)]
    [Tooltip("Tells if the totem can be activated only one time before a hard reset, like in muic puzzle for example.")]
    [SerializeField] bool interactOneTime = true;
    #endregion

    #region Private VARs
    private PuzzleLevel puzzleLevel;
    private ActiveAnim activeAnim;
    private bool isInteracted = false;
    #endregion

    #region Getters
    public bool IsInteracted
    {
        get { return isInteracted; }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        if (isPuzzleLevel)
            puzzleLevel = GetComponent<PuzzleLevel>();

        if (isActiveAnim)
            activeAnim = GetComponent<ActiveAnim>();
    }
    #endregion

    #region INTERFACE
    public void Interact()
    {
        if (isInteracted) return;

        if (interactOneTime)
            isInteracted = true;

        // puzzle activations
        if (musicPuzzle != null)
            musicPuzzle.TotemInteracted(this.gameObject);

        else if (puzzleLevel != null)
            puzzleLevel.ActivatedTotem();

        else if (activeAnim != null)
            activeAnim.ActivatedTotem();
    }
    #endregion

    #region Public Methods
    public void ResetInteracted()
    {
        isInteracted = false;
    }
    #endregion
}
