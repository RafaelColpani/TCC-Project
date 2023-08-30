using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerTutorial : MonoBehaviour
{
    [Header("Tutorial Config")] [Space(5)]
    [SerializeField] bool _activeTutorial = false;
    [SerializeField] float triggerCount = 0;

    [Header("Tutorial GameObject ")] [Space(5)]
    [SerializeField] GameObject tutorialFrame;

    [Header("Particle System ")] [Space(5)]
    [SerializeField] GameObject particleTutorial;

    // Start is called before the first frame update
    void Start()
    {
        tutorialFrame.SetActive(false);
        particleTutorial.SetActive(true);
    }

  

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        print("player vendo o tutorial");
        if(triggerCount <= 4)
        {
            tutorialFrame.SetActive(true);
            

            triggerCount ++;
        }

        if(triggerCount == 5)
        {
            particleTutorial.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        print("player saindo o tutorial");
     
        tutorialFrame.SetActive(false);

        
    }
}
