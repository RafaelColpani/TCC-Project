using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    [SerializeField] string nameScene;
    [SerializeField] float changeScene = 8f;

    void Start()
    {
        Invoke("NextScene", changeScene);
    }

    void NextScene()
    {
        SceneManager.LoadScene(nameScene);
    }
}
