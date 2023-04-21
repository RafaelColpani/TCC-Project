using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsMainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("LoadRoom");
    }

    public void LoadGame()
    {

    }

     public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();

    }

    public void OpenClose(bool b)
    {
        b = !b;
    }


}
