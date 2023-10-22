using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXMusic : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; // O componente de áudio que está tocando a música
    [SerializeField] GameObject[] objetosAtivaveis; // Os objetos que você deseja ativar em momentos específicos
    
    [SerializeField] [Tooltip("Os momentos tempo em segundos em que os objetos serão ativados")] 
    float[] momentosDeAtivacao; 

    private int momentoAtual = 0;
    private bool ativacaoIniciada = false;

    private void Update()
    {
        if (!ativacaoIniciada && audioSource.isPlaying)
        {
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
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        ativacaoIniciada = false;
        momentoAtual = 0;
        foreach (var vfx in objetosAtivaveis)
        {
            vfx.SetActive(false);
        }
    }
}
