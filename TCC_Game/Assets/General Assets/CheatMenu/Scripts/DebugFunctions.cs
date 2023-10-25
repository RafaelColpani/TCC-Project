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
    #endregion
}