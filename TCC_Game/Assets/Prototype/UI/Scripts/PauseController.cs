using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pause;
    [SerializeField] GameObject settings;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (pause.activeSelf)
            pause.SetActive(false);
        else
            pause.SetActive(true);
    }

    public void Settings()
    {
        if (settings.activeSelf)
            settings.SetActive(false);
        else
            settings.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Resume()
    {
        pause.SetActive(false);
        settings.SetActive(false);
        settings.GetComponentInChildren<TabGroup>().SelectFirstTab();
    }
}
