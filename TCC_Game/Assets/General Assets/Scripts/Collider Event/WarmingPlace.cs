using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using KevinCastejon.MoreAttributes;

public class WarmingPlace : MonoBehaviour
{
    [HeaderPlus(" ", "- COLLIDER AREA -", (int)HeaderPlusColor.yellow)]
    [SerializeField] [Range(0, 15)] float radiusInspector = 5f;
    private CircleCollider2D _collider;
    [Space(5)]

    [HeaderPlus(" ", "- TIMER -", (int)HeaderPlusColor.cyan)]
    [SerializeField] float initTimer = 25f;
    [SerializeField] float currentTimer;
    [SerializeField] float startVigTime = 35f;
    [Space(5)]

    [HeaderPlus(" ", "- VOLUME -", (int)HeaderPlusColor.magenta)]
    [SerializeField] Volume volume; 
    private Vignette vignette; 
    [SerializeField] float intenVignette = 0.05f;
    [Space(5)]

    [HeaderPlus(" ", "- SCENE CHANGE -", (int)HeaderPlusColor.green)]
    [SerializeField] GameObject uiAnimLoad;
    [SerializeField] float SecondsAnim = 2;
    [Space(5)]

    [HeaderPlus(" ", "- DEBUG -", (int)HeaderPlusColor.red)]
    [SerializeField] bool _isDebugMode = false;
 

    // Start is called before the first frame update
    void Start()
    {
        //Timer
        currentTimer = initTimer;

        //Post Process
        volume.profile.TryGet(out vignette);
        vignette.intensity.Override(0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Collider Area
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = radiusInspector;

        //Timer
        if (currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;

            //Post Process
            if(currentTimer <= startVigTime)
            {
                float intenVolume = vignette.intensity.value;
                intenVolume += intenVignette * Time.deltaTime;
                vignette.intensity.Override(intenVolume);
            }
        }

        else
        {
            Debug.LogError("Time");

            if(_isDebugMode == false) 
            {
                StartCoroutine(LoadAnim(SceneManager.GetActiveScene().buildIndex + 0));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogWarning("Player esta na area de calor");
            //Timer
            currentTimer = initTimer;

            //Post Process
            float intenVolume = vignette.intensity.value;
            intenVolume = Mathf.Clamp(intenVolume - intenVignette * Time.deltaTime, 0f, 1f);
            vignette.intensity.Override(intenVolume);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.LogWarning("Player esta fora da area de calor");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusInspector);
    }

    IEnumerator LoadAnim(int levelName)
    {
        uiAnimLoad.SetActive(true);

        yield return new WaitForSeconds(SecondsAnim);

        SceneManager.LoadScene(levelName);

        //uiAnimLoad.SetActive(false);
    }
}
