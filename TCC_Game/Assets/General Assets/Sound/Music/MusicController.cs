using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [TextArea]
    public string aviso = "Por favor, mantenha a correspond�ncia entre a cena e as m�sicas.";

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot @default;
    [SerializeField] AudioMixerSnapshot cave;
    [SerializeField] AudioMixerSnapshot paused;
    [SerializeField] AudioMixerSnapshot puzzleMusic;

    [SerializeField] GameObject pauseMenu;

    [SerializeField] float transitionTime = 5f;

    [SerializeField] string[] scenesNames;
    [SerializeField] int[] buildSceneIndexes;
    [SerializeField] int lastSceneReached; // serves to know where in the buildSceneIndex vector is the current or the next
    [SerializeField] AudioSource[] music;


    [SerializeField] int currentMusicIndex = 0;
    [SerializeField] float maxVolume;

    Transform playerTransform;

    [SerializeField] bool reverb;

    private void Awake()
    {
        
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        print("HANDLE SCENE LOADED ()");
        ChangeMusic();
    }


    void Start()
    {
        DontDestroyOnLoad(gameObject);

        GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
        for (int i = 0; i < gos.Length; i++)
        {
            if (gos[i].name.Contains("pause") && gos[i].GetComponent<Image>())
                pauseMenu = gos[i];
        }

        playerTransform = GameObject.FindGameObjectWithTag("TargetPlayer").transform;

        //ChangeMusic();
    }

    void Update()
    {   
        transform.parent.position = playerTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        for (int i = 0; i < scenesNames.Length; i++)
        {
                
            if (collision.gameObject.CompareTag("musicChange"))
            {
                if (collision.transform.name.Contains("reverb") || collision.transform.name.Contains("cave"))
                    CaveReverbSnapshot();
                else
                    DefaultSnapshot();
            }

            if (collision.gameObject.CompareTag("PuzzleMusic"))
            {
                PuzzleMusicSnapshot();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PuzzleMusic"))
        {
            print("puzzlemusic exit");
            if (!reverb)
                DefaultSnapshot();
            else
                CaveReverbSnapshot();
        }
    }

    //onsceneload

    void ChangeMusic()
    {
        for (int i = 0; i < buildSceneIndexes.Length; i++)
        {
            if(SceneManager.GetActiveScene().buildIndex == buildSceneIndexes[i])
            {
                music[currentMusicIndex].Stop();
                currentMusicIndex = SceneManager.GetActiveScene().buildIndex; //replaces old currentmusicindex with the new one
                music[i].Play();
                print($"builsceneindex: {SceneManager.GetActiveScene().buildIndex}, " +
                    $"current music: {music[i].gameObject}");
                break;
            }
        }
    }

    #region SFX Functions

    void DefaultSnapshot()
    {
        @default.TransitionTo(transitionTime);
    }

    void CaveReverbSnapshot()
    {
        cave.TransitionTo(transitionTime);
    }
    
    void PauseSnapshot()
    {
        if(pauseMenu.activeSelf)
            @default.TransitionTo(transitionTime);
        else
            paused.TransitionTo(transitionTime);
    }

    void PuzzleMusicSnapshot()
    {
        puzzleMusic.TransitionTo(0.5f);
    }

    #endregion
}
