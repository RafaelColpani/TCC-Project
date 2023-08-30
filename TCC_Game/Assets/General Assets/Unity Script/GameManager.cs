using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    [Header("FPS Display")]
    [SerializeField] TMP_Text fpsText;
    [SerializeField] TMP_Text fpsNameFrame;
    private int lastFPS;
    private float[] frameDeltaTime;

   

    // Start is called before the first frame update
    void Start()
    {
        //FPS Log
        frameDeltaTime = new float[50];
    }

    // Update is called once per frame
    void Update()
    {
        //FPS Log
        fpsNameFrame.enabled = true;
        fpsText.enabled = true;
        frameDeltaTime[lastFPS] = Time.deltaTime;
        lastFPS = (lastFPS + 1) % frameDeltaTime.Length;
        fpsText.text = Mathf.RoundToInt(CalculateFPS()).ToString();
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

    public void EnableFPSCounter(bool condition)
    {
        fpsNameFrame.transform.parent.gameObject.SetActive(condition);
        
    }
}
