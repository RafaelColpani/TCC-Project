using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    DialogueReader2 dialogueReader;
    public int nextId;
    private void Awake()
    {
        dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader2>();
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick() 
    {
        dialogueReader.Chose(nextId);
    }    
}
