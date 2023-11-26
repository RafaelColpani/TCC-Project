using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private static bool isPaused = false;

    [SerializeField] GameObject pause;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject dimBackground;
    [SerializeField] GameObject pretinhoSuave2;
    [SerializeField] GameObject firstSelected;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject firstSettingsSelected;

    private static bool wasPaused = false;
    private bool canResume = false;

    private PlayerActions playerActions;

    private void Awake()
    {
        isPaused = false;
        wasPaused = false;

        playerActions = new PlayerActions();
        playerActions.Movement.Pause.performed += ctx => Pause();
    }

    public void Pause()
    {
        if (pause.activeSelf)
        {
            print("despausou");
            if (settings.activeInHierarchy)
            {
                settings.GetComponentInChildren<TabGroup>().SelectFirstTab();
                settings.SetActive(false);
            }
            pause.SetActive(false);
            dimBackground.SetActive(false);
            canResume = false;
            Time.timeScale = 1;
            if (wasPaused) return;
            isPaused = false;
        }

        else
        {
            print("pausou");
            EventSystem.current.SetSelectedGameObject(firstSelected);
            pause.SetActive(true);
            dimBackground.SetActive(true);
            canResume = false;
            StartCoroutine(EnableResume());
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void Settings()
    {
        if (settings.activeSelf)
        {
            
            settings.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsButton);
        }

        else
        {

            settings.SetActive(true);
            var tabGroup = GetComponentInChildren<TabGroup>();
            tabGroup.OnTabSelected(tabGroup.firstTabSelected.GetComponent<_TabButton>());
            EventSystem.current.SetSelectedGameObject(firstSettingsSelected);
        }
    }

    public void MainMenu()
    {
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void Resume()
    {
        if (!canResume)
        {
            pretinhoSuave2.SetActive(false);
            return;
        }

        if (settings.activeInHierarchy)
        {
            settings.GetComponentInChildren<TabGroup>().SelectFirstTab();
            settings.SetActive(false);
            pretinhoSuave2.SetActive(false);
        }
        if (pause.activeSelf)
            Pause();
    }

    private IEnumerator EnableResume()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        canResume = true;
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

    #region Enable & Disable
    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
    #endregion
}
