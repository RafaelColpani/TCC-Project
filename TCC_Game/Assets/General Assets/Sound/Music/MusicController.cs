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
    [SerializeField] float maxVolume;

    Transform playerTransform;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("TargetPlayer").transform;
        music[currentMusicIndex].Play();
        for (int i = 0; i < music.Length; i++)
        {
            music[i].volume = 0f;
        }
        music[currentMusicIndex].volume = 1f;
        music[currentMusicIndex].Play();
        ChangeMusic();
    }

    void Update()
    {   
        transform.parent.position = playerTransform.position;
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

        //if(collision.tag == "Player")
        //{
        //    for (int i = 0; i < scenesNames.Length; i++)
        //    {
        //        if(collision.tag == "musicChange")
        //        {
        //            // vai servir mais para o futuro
        //            string[] splitArray = scenesNames[i].Split(" ");
        //            if (collision.transform.name.Contains(splitArray[0]) /*||
        //            collision.GetComponent<Load_Scene>()._nextScene.Contains(splitArray[1])*/)
        //            {
        //                currentMusicIndex = i;
        //                print($"currentMusicIndex: {currentMusicIndex}");
        //                ChangeMusic();
        //            }
        //        }
        //    }
        //}
    }

    void ChangeMusic()
    {
        for (int i = 0; i < music.Length; i++)
        {
            if (i != currentMusicIndex)
            {
                music[i].volume = 0f;
            }
            //music[currentMusicIndex].volume = 1f;
            music[currentMusicIndex].volume = 1f;
        }
    }
}
