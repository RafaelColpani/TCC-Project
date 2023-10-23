using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [TextArea]
    public string aviso = "Por favor, mantenha a correspondência entre a cena e as músicas.";

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot @default;
    [SerializeField] AudioMixerSnapshot cave;
    [SerializeField] AudioMixerSnapshot paused;
    [SerializeField] AudioMixerSnapshot puzzleMusic;

    [SerializeField] GameObject pauseMenu;

    [SerializeField] float transitionTime = 5f;

    [SerializeField] string[] scenesNames;
    [SerializeField] AudioSource[] music;
    [SerializeField] int currentMusicIndex = 0;
    [SerializeField] float maxVolume;

    Transform playerTransform;

    [SerializeField] bool reverb;
    void Start()
    {
        GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < gos.Length; i++)
        {
            if (gos[i].name.Contains("pause") && gos[i].GetComponent<Image>())
                pauseMenu = gos[i];
        }

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
                
            if (collision.gameObject.CompareTag("musicChange"))
            {

                if (collision.transform.name.Contains("reverb") || collision.transform.name.Contains("cave"))
                    CaveReverbSnapshot();
                else
                    DefaultSnapshot();
            }

            if (collision.gameObject.CompareTag("PuzzleMusic"))
            {
                print("puzzlemusic enter");
                PuzzleMusicSnapshot();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PuzzleMusic"))
        {
            print("puzzlemusic exit");
            DefaultSnapshot();
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
            
            //TO CHANGE TO A BOOL COMPARASION
            //if (reverb)
            //    CaveReverbSnapshot();
            //else
            //    DefaultSnapshot();

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

    void PuzzleMusicSnapshot()
    {
        puzzleMusic.TransitionTo(transitionTime/2);
    }

    #endregion
}
