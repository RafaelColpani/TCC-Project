using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugFunctions : MonoBehaviour
{
    #region Debug Commands
    private void Start()
    {
        DebugController.AddCommand("next_scene", "Avança uma cena", "next_scene", D_NextScene);
        DebugController.AddCommand("restart", "Recomeca a cena", "restart", D_Restart);
        DebugController.AddCommand("undo_scene", "Volta uma cena", "undo_scene", D_Undo);

        DebugController.AddCommand("scene_summer", "Va para o verao", "undo_scene", D_Summer);
        DebugController.AddCommand("scene_fall", "Va para o outono", "undo_scene", D_Fall);
        DebugController.AddCommand("scene_winter", "va para o inverno", "undo_scene", D_Winter);
    }
    #endregion

    #region Debug Functions
    public void D_NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void D_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void D_Undo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        
    }

    public void D_Summer()
    {
        SceneManager.LoadScene("Puzzle1");
        
    }

    public void D_Fall()
    {
        SceneManager.LoadScene("Autumn-Puzzle1");
        
    }

    public void D_Winter()
    {
        SceneManager.LoadScene("Winter-Tutorial");
        
    }
    #endregion
}