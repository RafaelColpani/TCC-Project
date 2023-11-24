using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using KevinCastejon.MoreAttributes;

public class WarmingManager : MonoBehaviour
{
    #region Inspector Vars
    [HeaderPlus(" ", "- TIMER -", (int)HeaderPlusColor.cyan)]
    [SerializeField] float initTimer = 25f;
    [SerializeField] float currentTimer;
    [SerializeField] float startVigTime = 35f;
    [Space(5)]

    [HeaderPlus(" ", "- VOLUME -", (int)HeaderPlusColor.magenta)]
    [SerializeField] Volume volume;
    private Vignette vignette;
    [SerializeField] float intenVignetteIN = 0.05f;
    [SerializeField] float intenVignetteON = 0.035f;
    [Space(5)]

    [HeaderPlus(" ", "- SCENE CHANGE -", (int)HeaderPlusColor.green)]
    [SerializeField] GameObject uiAnimLoad;
    [SerializeField] float SecondsAnim = 2;
    [Space(5)]

    [HeaderPlus(" ", "- DEBUG -", (int)HeaderPlusColor.red)]
    [SerializeField] bool _isDebugMode = false;
    #endregion

    #region Private Vars
    private bool isOnFire;
    #endregion

    #region Public Vars
    public bool IsOnFire
    {
        get { return isOnFire; }
        set { isOnFire = value; }
    }
    #endregion


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
        if (isOnFire) return;

        //Timer
        if (currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;

            //Post Process
            if (currentTimer <= startVigTime)
            {
                print("diminuindo vinhetinha");
                print($"current: {currentTimer} | init: {initTimer}");
                float intenVolume = vignette.intensity.value;
                intenVolume += intenVignetteIN * Time.deltaTime;
                vignette.intensity.Override(intenVolume);
            }
        }

        else
        {
            Debug.LogError("Time");

            if (_isDebugMode == false)
            {
                StartCoroutine(LoadAnim(SceneManager.GetActiveScene().buildIndex + 0));
            }
        }
    }

    public void StayOnFire()
    {
        Debug.LogWarning("Player esta na area de calor");
        //Timer
        currentTimer = initTimer;

        //Post Process
        float intenVolume = vignette.intensity.value;
        intenVolume = Mathf.Clamp(intenVolume - intenVignetteON * Time.deltaTime, 0f, 1f);
        vignette.intensity.Override(intenVolume);

        isOnFire = true;
    }

    IEnumerator LoadAnim(int levelName)
    {
        uiAnimLoad.SetActive(true);

        yield return new WaitForSeconds(SecondsAnim);

        SceneManager.LoadScene(levelName);

        //uiAnimLoad.SetActive(false);
    }
}
