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
        fadeOutObj.SetActive(true);
        var input = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<InputHandler>();
        input.GetJumpCommand().SetCanJump(false);
        input.canWalk = false;
        StartCoroutine(GoToGameOver());
    }

    IEnumerator GoToGameOver()
    {
        yield return new WaitForSeconds(timeToGameOver);

        SceneManager.LoadScene(gameOverScene);
    }
}
