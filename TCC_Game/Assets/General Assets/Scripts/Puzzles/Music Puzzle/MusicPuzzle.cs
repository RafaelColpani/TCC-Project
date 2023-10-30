using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using System.Linq;

public class MusicPuzzle : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- TOTEMS -", (int)HeaderPlusColor.green)]
    [Tooltip("Totems array in the puzzle")]
    [SerializeField] GameObject[] totems;

    [HeaderPlus(" ", "- PUZZLE LOGIC -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The correct sequence for resolve the puzzle, by the indexes of totems array")]
    [SerializeField] List<int> correctSequence;

    [HeaderPlus(" ", "- COMPLETED PUZZLE -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The script located in the object that starts the music for the puzzle")]
    [SerializeField] VFXMusic vfxMusic;
    [SerializeField] GameObject objetoParaAtivar;
    [SerializeField] GameObject objetoParaDesativar;
    [SerializeField] bool _MusicVictOnOff = false;
    [SerializeField] AudioClip victoryAudioClip; // Música a ser tocada quando o quebra-cabeça for resolvido

    [HeaderPlus(" ", "- VFX WIN -", (int)HeaderPlusColor.blue)]
    [SerializeField] GameObject shaderVFX;
    [SerializeField] Vector3 VFXScale = new Vector3(32, 32, 0);
    [SerializeField] float VFXTime = 1f;
    private bool _VFXLerp = false;

    #endregion

    #region Private VARs
    private AudioSource victoryAudioSource;

    private List<int> interactedSequence = new List<int>();

    private bool hasCompletedChallenge = false;
    #endregion

    #region Getters
    public bool HasCompletedChallenge
    {
        get { return hasCompletedChallenge; }
        set { hasCompletedChallenge = value; }
    }
    #endregion

    #region Unity Methods
    void Start()
    {
        ResetAllTotems();
        victoryAudioSource = GetComponent<AudioSource>();
        victoryAudioSource.playOnAwake = false;

        shaderVFX.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if(_VFXLerp == true)
        {
            var VFXLerp = new Vector3(1, 1, 0);
            shaderVFX.transform.localScale = Vector3.Lerp(shaderVFX.transform.localScale, VFXScale, VFXTime * Time.deltaTime);
        }
    }
    #endregion

    #region Public Methods
    public void TotemInteracted(GameObject totem)
    {
        int matchedIndex = -1;

        for (int i = 0; i < totems.Length; i++)
        {
            if (totems[i] != totem) continue;

            matchedIndex = i;
            break;
        }

        interactedSequence.Add(matchedIndex);

        if (interactedSequence.Count() >= correctSequence.Count())
            VerifySequence();
    }
    #endregion

    #region Private Methods
    private void VerifySequence()
    {
        for (int i = 0; i < interactedSequence.Count(); i++)
        {
            if (interactedSequence[i] == correctSequence[i]) continue;

            RestartChallenge();
            return;
        }

        CompletedChallenge();
    }

    void RestartChallenge()
    {
        ResetAllTotems();
        interactedSequence.Clear();
        Debug.Log("Sequência errada! Reiniciando o quebra-cabeça.");
    }

    /// <summary>The logic after successfully completed the challenge</summary>
    private void CompletedChallenge()
    {
        hasCompletedChallenge = true;

        vfxMusic.CompletedMusicPuzzle();
        Debug.Log("Quebra-cabeça resolvido! As alavancas corretas foram ativadas.");

        _VFXLerp = true;

        var spriteRenderVFX = shaderVFX.GetComponent<SpriteRenderer>();
            spriteRenderVFX.material.SetFloat("_ActiveOnOff", 1);
        
        objetoParaAtivar.SetActive(!objetoParaAtivar.activeSelf);
        objetoParaDesativar.SetActive(false);

        if (!_MusicVictOnOff || victoryAudioClip != null) return;

        victoryAudioSource.clip = victoryAudioClip;
        victoryAudioSource.Play();
    }

    void ResetAllTotems()
    {
        foreach (GameObject totem in totems)
        {
            totem.SetActive(true);
            totem.GetComponent<CustomActionInteraction>().ResetInteraction();
        }
    }
    #endregion
}
