using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    [TextArea]
    public string aviso = "Por favor, mantenha a correspondência entre a cena e as músicas.";

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot @default;
    [SerializeField] AudioMixerSnapshot cave;
    [SerializeField] AudioMixerSnapshot paused;
    [SerializeField] GameObject pauseMenu;

    [SerializeField] float transitionTime = 5f;

    [SerializeField] string[] scenesNames;
    [SerializeField] AudioSource[] music;
    [SerializeField] int currentMusicIndex = 0;
    [SerializeField] float maxVolume;

    Transform playerTransform;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("TargetPlayer").transform;
        
        for (int i = 0; i < music.Length; i++)
        {
            music[i].volume = 0f;
            music[i].Play();
        }
        music[currentMusicIndex].volume = 1f;
        ChangeMusic();
    }

    void Update()
    {   
        transform.parent.position = playerTransform.position;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseSnapshot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
        {
            for (int i = 0; i < scenesNames.Length; i++)
            {
                if (collision.name.Contains("Music"))
                {
                    if (collision.transform.name.Contains( i.ToString() ))
                    {
                        currentMusicIndex = i;
                        print($"--- currentMusicIndex: {currentMusicIndex}");
                        ChangeMusic();
                    }
                }
            }
    }

    void ChangeMusic()
    {
        for (int i = 0; i < music.Length; i++)
        {
            if (i != currentMusicIndex)
            {
                music[i].volume = 0f;
            }
            
            if (currentMusicIndex == 2)
                CaveReverbSnapshot();
            else
                DefaultSnapshot();

            music[currentMusicIndex].volume = 1f;
        }
    }

    #region SFX Functions

    void DefaultSnapshot()
    {
        @default.TransitionTo(transitionTime);
    }

    void CaveReverbSnapshot()
    {
        cave.TransitionTo(transitionTime);
    }
    
    void PauseSnapshot()
    {
        if(pauseMenu.activeSelf)
            @default.TransitionTo(transitionTime);
        else
            paused.TransitionTo(transitionTime);
    }

    #endregion
}
