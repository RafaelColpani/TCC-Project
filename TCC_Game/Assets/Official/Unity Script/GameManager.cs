using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    // Start is called before the first frame update
    void Start()
    {
        //Scenes
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Summer1-Prototype", LoadSceneMode.Additive));  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
