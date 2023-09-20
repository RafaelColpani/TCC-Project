using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Load_Scene : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] bool _cutScene;
    [SerializeField] bool _transition;

    [Header("Scenes Name")]
    [SerializeField] string sceneName;
    private string sceneNameButton = "Tutorial_1";


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_transition == true) 
        {
            
        }
    }

    #region Cutscenes
    void cutScene() 
    {
        if(_cutScene == true) 
        {
            
        }
    }

    public void cutSceneButton() 
    {
        SceneManager.LoadScene(sceneNameButton);
    }
    #endregion
}
