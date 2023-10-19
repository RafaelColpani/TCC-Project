using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlaybackController : MonoBehaviour
{
    [Header("Cutscene")]
    [SerializeField] VideoPlayer videoPlayer;
    private bool hasStarted = false;

    [Header("Scene")]
    [SerializeField] string sceneName;

    private void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Inscreva-se no evento de término do vídeo
        videoPlayer.loopPointReached += OnVideoEnd;
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
        SceneManager.LoadScene(sceneName);
    }
}
