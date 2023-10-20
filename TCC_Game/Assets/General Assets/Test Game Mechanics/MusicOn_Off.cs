using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]

public class MusicOn_Off : MonoBehaviour
{
    [Header("Music Config")]
    [SerializeField] AudioClip musica;
    [SerializeField] bool ativarMusica = true;
    private bool musicaTocada = false;
    private AudioSource audioSource;
    [Space(10)]

    [Header("VFX Config")]
    [SerializeField] bool _VFXOnOff = false;
    [SerializeField] GameObject vfxMusic;
    [Space(10)]

    [Header("Text Config")]
    [SerializeField] bool _TextOnOff = false;
    [SerializeField] TextMeshProUGUI textoTMP; // Adicione a referência ao TextMeshPro
    [Space(10)]

    [Header("Dica Config")]
    [SerializeField] bool _dicaOnOff = false;
    [SerializeField] GameObject objetoDica;
    [SerializeField] TextMeshProUGUI textoDica;

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

                if(_TextOnOff == true)
                {
                    // Ative o texto TMP
                    if (textoTMP != null)
                    {
                        textoTMP.gameObject.SetActive(true);
                    }
                }

                if(_VFXOnOff == true)
                {
                    if(vfxMusic != null)
                    {
                        vfxMusic.SetActive(true);
                    }
                }

                
                else
                {
                    // Se a música já foi tocada, reinicie-a
                    audioSource.Stop();
                    audioSource.Play();
                }
            }

            if(_dicaOnOff == true)
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

            if(vfxMusic != null)
            {
                vfxMusic.SetActive(false);
            }
        }
    }
}
