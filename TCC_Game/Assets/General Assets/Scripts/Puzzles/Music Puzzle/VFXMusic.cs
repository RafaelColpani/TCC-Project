using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class VFXMusic : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- PUZZLE -", (int)HeaderPlusColor.green)]
    [Tooltip("O componente de áudio que está tocando a música")]
    [SerializeField] AudioSource audioSource;
    [Tooltip("Os objetos que você deseja ativar em momentos específicos")]
    [SerializeField] GameObject[] objetosAtivaveis;
    [Tooltip("Os momentos tempo em segundos em que os objetos serão ativados")]
    [SerializeField] float[] momentosDeAtivacao;

    [HeaderPlus(" ", "- CINEMACHINES -", (int)HeaderPlusColor.green)]
    [Tooltip("Cinemachine do player que será desativada. Deixe nulo se não quiser isso.")]
    [SerializeField] GameObject playerVCam;
    [Tooltip("Cinemachine extra que será ativada. Deixe nulo se não quiser isso.")]
    [SerializeField] GameObject puzzleVCam;
    [Tooltip("An extra time until cinemachines get back to player vcam.")]
    [SerializeField] float extraTime;
    [Tooltip("The players input handler to block movement while waiting for cinemachine. " +
        "Leave it null if you dont want it.")]
    [SerializeField] InputHandler inputHandler;
    #endregion

    #region Private VARs
    private int momentoAtual = 0;
    private bool ativacaoIniciada = false;
    private bool completedPuzzle = false;
    #endregion

    #region Getters
    public bool CompletedPuzzle { get { return completedPuzzle; } }
    #endregion

    #region Unity Methods
    private void Update()
    {
        if (PauseController.GetIsPaused()) return;

        ActivateLights();
    }
    #endregion

    #region Private Methods
    private void ActivateLights()
    {
        if (ativacaoIniciada || !audioSource.isPlaying) return;

        // Verifica se a música está tocando e se ainda não começou a ativação
        float tempoAtual = audioSource.time;
        

        if (momentoAtual < momentosDeAtivacao.Length && tempoAtual >= momentosDeAtivacao[momentoAtual])
        {
            // Ativa o objeto no momento especificado
            objetosAtivaveis[momentoAtual].SetActive(true);
            momentoAtual++;
        }

        if (momentoAtual >= momentosDeAtivacao.Length)
        {
            // Todos os momentos de ativação foram usados
            ativacaoIniciada = true;

            if (playerVCam != null && puzzleVCam != null)
            {
                StartCoroutine(BackToPlayerCam());
            }
        }
    }
    #endregion

    #region Public Methods
    public void CompletedMusicPuzzle()
    {
        completedPuzzle = true;
    }
    #endregion

    #region Unity Events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (completedPuzzle) return;

        if (playerVCam != null && puzzleVCam != null)
        {
            if (inputHandler != null)
            {
                inputHandler.canWalk = false;
                inputHandler.GetJumpCommand().SetCanJump(false);
            }

            playerVCam.SetActive(false);
            puzzleVCam.SetActive(true);
        }

        ativacaoIniciada = false;
        momentoAtual = 0;
        foreach (var vfx in objetosAtivaveis)
        {
            vfx.SetActive(false);
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator BackToPlayerCam()
    {
        yield return new WaitForSeconds(extraTime);

        playerVCam.SetActive(true);
        puzzleVCam.SetActive(false);
        if (inputHandler != null)
        {
            inputHandler.canWalk = true;
            inputHandler.GetJumpCommand().SetCanJump();
        }
    }
    #endregion
}
