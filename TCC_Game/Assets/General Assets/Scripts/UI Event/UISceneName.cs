using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UISceneName : MonoBehaviour
{    
    [SerializeField] TextMeshProUGUI textMeshPro;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        textMeshPro.text = "Cena Atual: \n" + sceneName;
    }
}
