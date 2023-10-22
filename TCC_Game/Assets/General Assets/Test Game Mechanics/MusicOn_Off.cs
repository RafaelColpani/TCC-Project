using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]

public class MusicOn_Off : MonoBehaviour
{
    #region Inspector Vars
    [Header("Music Config")]
    [SerializeField] AudioClip musica;
    [SerializeField] bool stopMusicInExit = true;
    
    [Space(10)]

    [Header("VFX Config")]
    [SerializeField] bool _VFXOnOff = false;
    [SerializeField] GameObject vfxMusic;

    [Space(10)]

    [Header("Text Config")]
    [SerializeField] bool _TextOnOff = false;
    [SerializeField] TextMeshProUGUI textoTMP;

    [Space(10)]

    [Header("Dica Config")]
    [SerializeField] bool _dicaOnOff = false;
    [SerializeField] GameObject objetoDica;
    [SerializeField] TextMeshProUGUI textoDica;
    #endregion

    #region Private Vars
    private AudioSource audioSource;
    #endregion

    #region Unity Methods
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Desative o texto TMP no início
        if (textoTMP != null)
            textoTMP.gameObject.SetActive(false);
    }
    #endregion

    #region Unity Events
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (audioSource == null || musica == null) return;
            
        audioSource.clip = musica;
        audioSource.Play();

        // text activate
        if(_TextOnOff && textoTMP != null)
            textoTMP.gameObject.SetActive(true);

        // vfx activate
        if(_VFXOnOff && vfxMusic != null)
            vfxMusic.SetActive(true);

        if(_dicaOnOff)
        {   
            objetoDica.SetActive(true);
            textoDica.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (textoTMP != null)
            textoTMP.gameObject.SetActive(false);

        if(vfxMusic != null)
            vfxMusic.SetActive(false);

        if (stopMusicInExit)
            audioSource.Stop();
    }
    #endregion
}
