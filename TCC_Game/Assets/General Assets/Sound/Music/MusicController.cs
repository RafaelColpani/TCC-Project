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
    [SerializeField] int[] stopMusicBuildIndexes;
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
        print("HANDLE SCENE LOADED: "+ SceneManager.GetActiveScene().buildIndex);

        if (SceneManager.GetActiveScene().buildIndex == 0)
            music[currentMusicIndex].Stop();
        else
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

        playerTransform = GameObject.FindWithTag("TargetPlayer").transform;

        //ChangeMusic();
    }

    void Update()
    {
        if (GameObject.FindWithTag("TargetPlayer"))
            playerTransform = GameObject.FindWithTag("TargetPlayer").transform;

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
        foreach (var i in stopMusicBuildIndexes)
        {
            if (SceneManager.GetActiveScene().buildIndex == i)
            {
                music[currentMusicIndex].Stop();
                return;
            }
        }

        for (int i = 0; i < buildSceneIndexes.Length; i++)
        {
            if (SceneManager.GetActiveScene().buildIndex == buildSceneIndexes[i])
            {
                if (i > 0)
                    music[i-1].Stop();

                currentMusicIndex = i; //replaces old currentmusicindex with the new one
                if (music[currentMusicIndex])
                {
                    music[currentMusicIndex].Play();
                    print($"builsceneindex: {SceneManager.GetActiveScene().buildIndex}, " +
                        $"current music: {music[currentMusicIndex].gameObject}");
                }

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
