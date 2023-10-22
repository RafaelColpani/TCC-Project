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
    [SerializeField] GameObject objetoParaAtivar;
    [SerializeField] GameObject objetoParaDesativar;
    [SerializeField] bool _MusicVictOnOff = false;
    [SerializeField] AudioClip victoryAudioClip; // Música a ser tocada quando o quebra-cabeça for resolvido
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
    }
    #endregion

    #region Public Methods
    public void TotemInteracted(GameObject totem)
    {
        int matchedIndex = -1;

        for (int i = 0; i < totems.Length; i++)
        {
            if (totems[i] == totem)
            {
                matchedIndex = i;
                break;
            }

            if (i >= totems.Length - 1)
            {
                Debug.LogError("Cannot find the correct totem music for the puzzle.");
                return;
            }
        }

        interactedSequence.Add(matchedIndex);
        if (interactedSequence.Count() >= correctSequence.Count())
            VerifySequence();
    }
    #endregion

    #region Private Methods
    private void VerifySequence()
    {
        foreach (var i in interactedSequence)
        {
            print($"catchedSeq: {i}");
        }

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

        Debug.Log("Quebra-cabeça resolvido! As alavancas corretas foram ativadas.");
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
            totem.GetComponent<MusicTotemInteract>().ResetInteracted();
        }
    }
    #endregion
}
