using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractable : MonoBehaviour, IInteractable
{
    [HideInInspector] public int timesTalked = 0;
    [SerializeField] string fileName = "dialogue.json";
    [SerializeField] GameObject dialogueManager, dialogueGrp;
    DialogueReader2 dialogueReader;

   /* private void Awake()
    {
        dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader2>();
    }*/
    public void Interact()
    {
        timesTalked++;

        dialogueManager.SetActive(true);
        dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader2>();
        dialogueReader.fileName = fileName;
        dialogueReader.npcInteract = this;

        dialogueGrp.SetActive(true);
    }
}
