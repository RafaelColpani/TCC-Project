using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WarmingPlace : MonoBehaviour
{
    [Header("Collider Area")]
    private CircleCollider2D _collider;
    [SerializeField] [Range(0, 15)] float radiusInspector = 5f;
    [Space(5)]

    [Header("Timer")]
    [SerializeField] float initTimer = 25f;
    [SerializeField] float currentTimer;
    [Space(5)]

    [Header("UI - Prov")]
    [SerializeField] Volume _volume;
    private Vignette m_Vignette;
    [Space(5)]

    [Header("Scene Change")]
    [SerializeField] GameObject uiAnimLoad;
    [SerializeField] float SecondsAnim = 2;
    [Space(5)]

    [Header("Debug")]
    [SerializeField] bool _isDebugMode = false;
 

    // Start is called before the first frame update
    void Start()
    {
        //Timer
        currentTimer = initTimer;

        _volume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        //Collider Area
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = radiusInspector;

        //Timer
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;


            m_Vignette.intensity.value = Mathf.Sin(currentTimer + Time.realtimeSinceStartup);
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
            Debug.LogWarning("Player está na area de calor");
            //Timer
            currentTimer = initTimer;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.LogWarning("Player está fora da area de calor");
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
