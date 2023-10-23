using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Load_Scene : MonoBehaviour
{
    [SerializeField] string sceneName; // Nome da cena de destino

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TrocarParasceneName();
        }
    }

    public void TrocarParasceneName()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Cena de destino não especificada no Inspector.");
        }
    }
}
