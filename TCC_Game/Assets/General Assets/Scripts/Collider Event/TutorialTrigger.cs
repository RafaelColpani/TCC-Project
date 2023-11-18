using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;


public class TutorialTrigger : MonoBehaviour
{
    [HeaderPlus(" ", "- Tutorial GameObject -", (int)HeaderPlusColor.yellow)]
    [SerializeField] GameObject tutorialObject; 

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialObject.SetActive(false);
        }
    }
   
}
