using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Load_Scene : MonoBehaviour
{
    [SerializeField] string sceneName; // Nome da cena de destino

    [SerializeField] bool _OnOFFAnim = false;
    [SerializeField] GameObject uiAnimLoad; // Nome da cena de destino
    [SerializeField] float SecondsAnim = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TrocarParasceneName();
        }
    }

    public void TrocarParasceneName()
    {
        if (!string.IsNullOrEmpty(sceneName) && _OnOFFAnim == true)
        {
            StartCoroutine(LoadAnim(SceneManager.GetActiveScene().buildIndex + 1));
        }

        if (!string.IsNullOrEmpty(sceneName) && _OnOFFAnim == false)
        {
            SceneManager.LoadScene(sceneName);
        }

        else
        {
            Debug.LogWarning("Cena de destino não especificada no Inspector.");
        }
    }

    IEnumerator LoadAnim(int levelName)
    {
        uiAnimLoad.SetActive(true);

        yield return new WaitForSeconds(SecondsAnim);

        SceneManager.LoadScene(levelName);

        //uiAnimLoad.SetActive(false);
    }
}
