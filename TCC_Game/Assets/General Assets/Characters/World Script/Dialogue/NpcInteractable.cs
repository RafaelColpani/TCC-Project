using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NpcInteractable : MonoBehaviour, IInteractable
{
    [HideInInspector] public static int timesTalked = 0;
    [HideInInspector] public bool canTalk = true;

    [SerializeField] string fileName = "dialogue.json";
    [SerializeField] GameObject dialogueManager, dialogueGrp;
    DialogueReader dialogueReader;

    /*
     private void Awake()
     {
        dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader>();
     }
    */

    public void Interact()
    {
        Debug.Log("Into Interact void");
        PauseController.SetPause();

        if (canTalk)
        {
            timesTalked++;

            dialogueManager.SetActive(true);
            dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader>();
            dialogueReader.fileName = fileName;
            dialogueReader.npcInteract = this;

            dialogueGrp.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);

            dialogueReader.StartAll();
            canTalk = false;
        }
    }
}
