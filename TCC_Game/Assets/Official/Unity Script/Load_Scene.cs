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
    [SerializeField] float loadRange = 5f;

    //States
    [SerializeField] bool isLoaded;
    [SerializeField] bool shouldLoad;

    [Space(5)]
    [Header("Gizmo")]
    [SerializeField] Color colorGizmo = Color.black;
    [SerializeField] BoxCollider2D box;


    // Start is called before the first frame update
    void Start()
    {
        //Debug
        box = gameObject.GetComponent<BoxCollider2D>();

        //Load Scene
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

    private void OnDrawGizmos()
    {
        //Player
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.position);

        //BoxArea
        Gizmos.color = colorGizmo;
        box = gameObject.GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(new Vector3(box.offset.x + transform.position.x, box.offset.y + transform.position.y, 1), new Vector3(box.size.x, box.size.y, 1));
    }
}
