using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load_Scene : MonoBehaviour
{
    /// <summary> Pr�ximao cena a ser carregada. A vari�vel est� exposta publicamente pois est� sendo usada pelo MusicController.</summary>
    [SerializeField] public string _nextScene;
    [SerializeField] string lastScene;
    public static bool load;

    private GameObject player;
    [SerializeField] Vector3 playerPos;


    //States
    //[SerializeField] bool _normalScene = false;
    [SerializeField] bool _additiveScene = false;

    /*
    [Space(5)]
    [Header("Gizmo")]
    [SerializeField] Color colorGizmo = Color.black;
    [SerializeField] PolygonCollider2D box;
    */

    [Header("UI")]
    private GameObject colliderScene;
    private Slider sliderUI;

    void Update()
    {
        colliderScene = GameObject.Find("--- UI/Canvas_UI/sld_loadScene");
        sliderUI = colliderScene.GetComponentInChildren<Slider>();
        
        colliderScene.SetActive(false);

        player = GameObject.FindGameObjectWithTag("TargetPlayer");

        playerPos = player.GetComponent<Transform>().position;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        /*
        if (_normalScene == true)
        {
            
                print("normal scene ok");
                //SceneManager.UnloadScene(lastScene);
                //SceneManager.LoadScene("LoadRoom", LoadSceneMode.Additive);
                SceneManager.LoadScene(_nextScene, LoadSceneMode.Additive);
            
        }
        */

        if (_additiveScene == true)
        {
            if(collision.gameObject.name == "Scene Collider" && !SceneManager.GetSceneByName(_nextScene).isLoaded) 
            {
                print("_additiveScene ok");
                
                /*
                if (load) 
                {
                    load = false;
                    print("cu azul");
                    return;
                }
                */

                CustomLoad(_nextScene);
                //AsyncOperation loadScene =  SceneManager.LoadSceneAsync(_nextScene, LoadSceneMode.Additive);
                load = true;
            }
        }

        void CustomLoad(string _nextScene)
        {
            StartCoroutine(LoadAsynScene(_nextScene));
        }

        IEnumerator LoadAsynScene(string _nextScene)
        {
            AsyncOperation loadScene =  SceneManager.LoadSceneAsync(_nextScene, LoadSceneMode.Additive);

            colliderScene.SetActive(true);
            //print("colliderScene Active:" + " " + colliderScene);

            while(!loadScene.isDone){
                float progress = Mathf.Clamp01(loadScene.progress / .9f);

                sliderUI.value = progress;
              
                //print(progress);
                //print("Slider Active:" + " " + sliderUI);

                if(progress == 1)
                {
                    colliderScene.SetActive(false);
                }

                yield return null;
            }  
        }
    }

    /*
    private void OnDrawGizmos()
    {
        //Player
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.position);

        //BoxArea
        Gizmos.color = colorGizmo;
        box = gameObject.GetComponent<PolygonCollider2D>();
        Gizmos.DrawWireCube(new Vector3(box.offset.x + transform.position.x, box.offset.y + transform.position.y, 1), new Vector3(box..x, box.size.y, 1));
    }
    */
}
