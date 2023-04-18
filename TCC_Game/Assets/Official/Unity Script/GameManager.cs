using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Dev Mode")]
    [SerializeField] bool devMode;
    [SerializeField] TMP_Text devText;

    [Space(5)]
    [Header("FPS Display")]
    [SerializeField] TMP_Text fpsText;
    [SerializeField] TMP_Text fpsNameFrame;
    private int lastFPS;
    private float[] frameDeltaTime;

    [Space(5)]
    [Header("Scene")]
    [SerializeField] TMP_Text sceneNameFrame;
    [SerializeField] TMP_Text sceneText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager Start" + " " + " [GameManager.cs] ");

        //FPS Log
        frameDeltaTime = new float[50];
    }

    // Update is called once per frame
    void Update()
    {
        //Dev mode log
        if(devMode)
        {
            devText.enabled = true;

            //FPS Log
            fpsNameFrame.enabled = true;
            fpsText.enabled = true;
            
            frameDeltaTime[lastFPS] = Time.deltaTime;
            lastFPS = (lastFPS + 1) % frameDeltaTime.Length;
            fpsText.text = Mathf.RoundToInt(CalculateFPS()).ToString();

            //Scene Log
            sceneNameFrame.enabled = true;
            sceneText.enabled = true;
            
            Scene scene = SceneManager.GetActiveScene();
            sceneText.text = scene.name;

        }

        if(!devMode)
        {
            sceneNameFrame.enabled = false;
            fpsNameFrame.enabled = false;
            devText.enabled = false;
            sceneText.enabled = false;
            fpsText.enabled = false;
        }
    }

    float CalculateFPS()
    {
        float totalFPS = 0f;
        foreach (float deltaTime in frameDeltaTime)
        {
            totalFPS += deltaTime;
        }

        return frameDeltaTime.Length / totalFPS;
    }
}
