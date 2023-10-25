using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private static bool isPaused = false;

    [SerializeField] GameObject pause;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject dimBackground;
    private static bool wasPaused = false;

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
        {
            print("despausou");
            pause.SetActive(false);
            dimBackground.SetActive(false);
            Time.timeScale = 1;
            if (wasPaused) return;
            PauseController.isPaused = false;
        }
        else
        {
            print("pausou");
            pause.SetActive(true);
            dimBackground.SetActive(true);
            Time.timeScale = 0;
            PauseController.isPaused = true;
        }
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
        PauseController.isPaused = false;
        Time.timeScale = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene("Main Menu");
    }

    public void Resume()
    {
        settings.SetActive(false);
        settings.GetComponentInChildren<TabGroup>().SelectFirstTab();
        Pause();
    }

    public static bool GetIsPaused()
    {
        return isPaused;
    }

    public static void SetPauseAndTime(bool pause = true)
    {
        isPaused = pause;

        if (pause)
            Time.timeScale = 0;

        else
            Time.timeScale = 1;
    }

    public static void SetPause(bool value = true)
    {
        isPaused = value;
        wasPaused = value;
    }
}
