using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum enumMethod
{
    Distance,
    Trigger
}

public class Load_Scene : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] enumMethod _enumMethod;
    [SerializeField] float loadRange;

    //States
    [SerializeField] bool isLoaded;
    [SerializeField] bool shouldLoad;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name)
                {
                    isLoaded = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_enumMethod == enumMethod.Distance)
        {
            DistanceCheck();
        }

        if (_enumMethod == enumMethod.Trigger)
        {
            TriggerCheck();
        }
    }

    void DistanceCheck()
    {
        if (Vector2.Distance(player.position, transform.position) < loadRange)
        {
            LoadScene();
        }

        else
        {
            UnloadScene();
        }
    }

    void TriggerCheck()
    {
        if (shouldLoad)
        {
            LoadScene();
        }

        else
        {
            UnloadScene();
        }
    }

    void LoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    void UnloadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = false;
        }
    }

}
