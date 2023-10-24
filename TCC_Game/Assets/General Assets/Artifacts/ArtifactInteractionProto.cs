using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtifactInteractionProto : MonoBehaviour, IInteractable
{
    private readonly string gameOverScene = "GameOver";
    [SerializeField] GameObject fadeOutObj;
    [SerializeField] float timeToGameOver;

    public void Interact()
    {
        SceneManager.LoadScene(gameOverScene);
        
        fadeOutObj.SetActive(true);
        //Debug.Log("4 linha antes da Coroutine"); - Não entra aqui

        /*
        var input = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<InputHandler>();
        Debug.Log("3 linha antes da Coroutine");
        input.GetJumpCommand().SetCanJump(false);
        Debug.Log("2 linha antes da Coroutine");
        input.canWalk = false;
        Debug.Log("1 linha antes da Coroutine");
        StartCoroutine(GoToGameOver());
        */
    }

    IEnumerator GoToGameOver()
    {
        yield return new WaitForSeconds(timeToGameOver);
        SceneManager.LoadScene(gameOverScene);

    }
}
