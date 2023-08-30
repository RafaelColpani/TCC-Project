using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;


public class MusicController : MonoBehaviour
{
    [TextArea]
    public string aviso = "Por favor, mantenha a correspondência entre a cena e as músicas.";

    [SerializeField] string[] scenesNames;
    [SerializeField] AudioSource[] music;
    [SerializeField] int currentMusicIndex = 0;

    void Start()
    {
        music[currentMusicIndex].Play();
        ChangeMusic();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            for (int i = 0; i < scenesNames.Length; i++)
            {
                

                if(collision.tag == "musicChange")
                {
                    // vai servir mais para o futuro
                    string[] splitArray = scenesNames[i].Split(" ");
                    if (collision.transform.name.Contains(splitArray[0]) /*||
                    collision.GetComponent<Load_Scene>()._nextScene.Contains(splitArray[1])*/)
                    {
                        currentMusicIndex = i;
                        print($"currentMusicIndex: {currentMusicIndex}");
                        ChangeMusic();
                    }
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
                music[i].mute = true;
            }
        }
    }
}
