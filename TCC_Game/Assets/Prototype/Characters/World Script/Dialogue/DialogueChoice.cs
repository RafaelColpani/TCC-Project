using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    DialogueReader dialogueReader;
    public int nextId;
    private void Awake()
    {
        dialogueReader = GameObject.Find("dialogueManager").GetComponent<DialogueReader>();
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick() 
    {
        dialogueReader.Chose(nextId);
    }    
}
