using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]

public class MusicOn_Off : MonoBehaviour
{
    public AudioClip musica;
    public bool ativarMusica = true;
    private bool musicaTocada = false;
    private AudioSource audioSource;
    public TextMeshProUGUI textoTMP; // Adicione a referência ao TextMeshPro

    public bool _dicaOnOff = false;
    public GameObject objetoDica;
    public TextMeshProUGUI textoDica;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Desative o texto TMP no início
        if (textoTMP != null)
        {
            textoTMP.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ativarMusica && other.CompareTag("Player"))
        {
            if (audioSource != null && musica != null)
            {
                if (!musicaTocada)
                {
                    audioSource.clip = musica;
                    audioSource.Play();
                    musicaTocada = true;

                    
                }

                // Ative o texto TMP
                if (textoTMP != null)
                {
                    textoTMP.gameObject.SetActive(true);
                }

                
                else
                {
                    // Se a música já foi tocada, reinicie-a
                    audioSource.Stop();
                    audioSource.Play();
                }
            }

            if(_dicaOnOff = true)
            {   
            
                {
                    objetoDica.SetActive(true);
                    textoDica.gameObject.SetActive(true);
                }

            }
        }

        

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Desative o texto TMP quando o jogador sair do Collider Trigger
            if (textoTMP != null)
            {
                textoTMP.gameObject.SetActive(false);
            }
        }
    }
}
