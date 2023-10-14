using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]

public class MusicOn_Off : MonoBehaviour
{
    public AudioClip musica;
    public bool ativarMusica = true;
    private bool musicaTocada = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
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
                else
                {
                    // Se a música já foi tocada, reinicie-a
                    audioSource.Stop();
                    audioSource.Play();
                }
            }
        }
    }
}
