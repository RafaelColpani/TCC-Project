using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractable : MonoBehaviour, IInteractable
{
    [HideInInspector] public int timesTalked = 0;
    [SerializeField] string fileName = "dialogue.json";
    [SerializeField] GameObject dialogueManager, dialogueGrp;
    DialogueReader dialogueReader;
    [HideInInspector] public bool canTalk = true;
   /*
    private void Awake()
    {
        dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader>();
    }
   */
    public void Interact()
    {
        Debug.Log("aaaaaa");

        if(canTalk){
        timesTalked++;

        dialogueManager.SetActive(true);
        dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader>();
        dialogueReader.fileName = fileName;
        dialogueReader.npcInteract = this;

        dialogueGrp.SetActive(true);

        dialogueReader.StartAll();
        canTalk = false;
        }
    }
}
