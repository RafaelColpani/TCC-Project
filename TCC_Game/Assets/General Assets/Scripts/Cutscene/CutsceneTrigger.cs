using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] GameObject PlayerBody;
    [SerializeField] GameObject NPCBody;
    //[SerializeField] float numEnt = 0;

    [Header("Cutscene")]
    [SerializeField] GameObject videoGameObject;
    [SerializeField] VideoPlayer videoPlayer;
    private bool hasStarted = false;

    [Header("Cameras Virtuais")]
    [SerializeField] GameObject playerVCam;
    [SerializeField] GameObject eventVCam;

    private void Start()
    {
        videoGameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Player e NPC estao na range");

            //Game Object
            videoGameObject.SetActive(true);

            //Cam Virtual
            playerVCam.SetActive(false);
            eventVCam.SetActive(true);

            //videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void Update()
    {
        if (!hasStarted && videoPlayer.isPlaying)
        {
            hasStarted = true;
            Debug.Log("O vídeo começou a tocar.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("O vídeo terminou de tocar.");
       
        //Cam VM
        playerVCam.SetActive(true);
        eventVCam.SetActive(false);

        videoGameObject.SetActive(false);
    }

}
