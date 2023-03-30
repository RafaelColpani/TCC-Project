using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DialogueReader2 : MonoBehaviour
{
    #region external variables
    public float speed = 0.5f;
    [SerializeField] GameObject dialogueGrp;
    public string fileName = "dialogue.json";
    [SerializeField] TMP_Text name, dialogue;
    [SerializeField] GameObject choicesBox;
    [SerializeField] GameObject choicesButton;
    #endregion

    #region internal variables
    private DialogueData dialogueData;
    private int id;
    [HideInInspector]public NpcInteractable npcInteract;
    List<GameObject> buttons = new List<GameObject>();
    [HideInInspector]public Collider2D npcInteractCollider;
    #endregion


    private void Update()
    {
        
        //if it is clicked, it checks whether the text in the dialogue box is the same as what is expected to be written
        //if it is, it goes to the next line. if not, it completes the dialogue and checks if it must show choices
            if (Input.GetMouseButtonDown(0))
            {
                if (dialogue.text == dialogueData.dialogue[id].text)
                {
                    if(choicesBox.activeSelf == false)//player can only skip if there isn't any choice box
                    {
                        NextLine();
                    }
                }
                else
                {
                    StopAllCoroutines();
                    dialogue.text = dialogueData.dialogue[id].text;

                    if (dialogueData.dialogue[id].choices.Count > 0 && choicesBox.activeSelf == false)
                    {
                        print("choices: "+dialogueData.dialogue[id].choices[0]);
                        ShowChoices();
                    }
                }
            }
    }
    public void StartAll()
    {
        //finds path to the json file to be read
        string filePath = Path.Combine(Application.dataPath,"JSON", fileName);

        //if filepath exists, it is read and assimilated to the DialogueData script
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            dialogueData = JsonUtility.FromJson<DialogueData>(dataAsJson);
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }

        //Starts dialogue
        StartDialogue();
    }

    void StartDialogue()
    {
        //if it is the first time player interacts with npc, it gets the first line's id
        if (npcInteract.timesTalked <= 1)
        {
            id = 0;
            print("first time dialogue");
        }
        else //if not, it gets another id
        {
            id = 2;
            print("second time dialogue");
        }

            ClearAndTalk();

        if (dialogueData.dialogue[id].choices.Count > 0) //if there are choices, show these choices
        {
            print("choices: "+dialogueData.dialogue[id].choices.Count);
            ShowChoices();
        }
    }

    void ClearAndTalk()
    {
        name.text = dialogueData.dialogue[id].character;
            dialogue.text = string.Empty;
        StartCoroutine(TypeLine(dialogueData.dialogue[id].text)); //types the text of the id's line
    }
   
    void NextLine()
    {
        if (id < dialogueData.dialogue.Count - 1) //checks if dialogues are not finished
        {
            //checks if there is a nextId
            if (dialogueData.dialogue[id].nextId == 0) //if there isn't, id adds 1
            {
                id++;
            }
            else //if there is, it gets the next id
            {
                id = dialogueData.dialogue[id].nextId;
            }
            ClearAndTalk();
        }
        else//if it is, disable dialogue UI and manager, and sets id back to 0
        {
            npcInteract.canTalk = true;
            dialogueGrp.SetActive(false);
            id = 0;
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine(string txt)
    {
        foreach (char c in txt.ToCharArray())
        {
            dialogue.text += c;

            yield return new WaitForSeconds(speed);
        }

        if (dialogue.text == txt)
        {
            if (dialogueData.dialogue[id].choices.Count > 0 && choicesBox.activeSelf == false)
            {
                print("choices: "+dialogueData.dialogue[id].choices[0]);
                ShowChoices();
            }
        }
    }

    #region choices
    void ShowChoices() 
    {
        choicesBox.SetActive(true);
        foreach (var choice in dialogueData.dialogue[id].choices) 
        {
            GameObject ch = Instantiate(choicesButton, choicesBox.transform);
            buttons.Add(ch);
            ch.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            ch.GetComponent<DialogueChoice>().nextId = choice.nextId;
        }
    }

    void DestroyChoices() 
    {
        foreach (GameObject child in buttons) 
        {
            Destroy(child);
        }
        buttons.Clear();
        choicesBox.SetActive(false);
    }

    public void Chose(int choiceNextId) 
    {
        //gets id of the next dialogue accordingly to the choice
        id = choiceNextId;//dialogueData.dialogue[id].choices[choiceId].nextId; 
        //then, it destroy the choices and hides the choicesBox
        DestroyChoices();
        //goes to the nextLine
        //NextLine();
        ClearAndTalk();
    }
    #endregion
}
