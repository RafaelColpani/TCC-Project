using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

public class DialogueReader : MonoBehaviour
{
    #region external variables
    public float speed = 0.5f;

    [SerializeField]
    GameObject dialogueGrp;
    public string fileName = "dialogue.json";

    [SerializeField]
    TMP_Text name,
        dialogue;

    [SerializeField]
    GameObject choicesBox;

    [SerializeField]
    GameObject choicesButton;
    #endregion

    #region internal variables
    private DialogueData dialogueData;
    private int id;

    [HideInInspector]
    public NpcInteractable npcInteract;
    List<GameObject> buttons = new List<GameObject>();

    [HideInInspector]
    public Collider2D npcInteractCollider;
    DialogueConditions dialogueConditions = new DialogueConditions();
    List<Condition> conditions;
    bool alreadyTyping;
    #endregion

    private int teste = 0;

    private void Update()
    {
        //if it is clicked, it checks whether the text in the dialogue box is the same as what is expected to be written
        //if it is, it goes to the next line. if not, it completes the dialogue and checks if it must show choices
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogue.text.Equals(dialogueData.dialogue[id].text))
            {
                if (choicesBox.activeSelf == false) //player can only skip if there isn't any choice box
                {
                    print("to next line");
                    NextLine();
                }
            }
            else
            {
                StopAllCoroutines();
                alreadyTyping = false;
                dialogue.text = dialogueData.dialogue[id].text;

                if (dialogueData.dialogue[id].choices.Count > 0 && choicesBox.activeSelf == false)
                {
                    ShowChoices();
                }
            }
        }
    }

    public void StartAll()
    {
        //finds path to the json file to be read
        string filePath = Path.Combine(Application.dataPath, "JSON", fileName);

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

        //pega a lista de condi��es
        conditions = dialogueConditions.TurnToConditions(npcInteract.timesTalked <= 1);
        //Starts dialogue
        StartDialogue();
    }

    void StartDialogue()
    {
        id = 0;
        //if it is not the first time player interacts with npc, it gets right to the choices
        if (npcInteract.timesTalked > 1)
        {
            NextLine();
        }
        else
        {
            ClearAndTalk();
        }

        if (dialogueData.dialogue[id].choices.Count > 0) //if there are choices, show these choices
        {
            ShowChoices();
        }
    }

    void ClearAndTalk()
    {
        name.text = dialogueData.dialogue[id].character;
        dialogue.text = string.Empty;
        StartCoroutine(TypeLine(dialogueData.dialogue[id].text)); //types the text of the id's line}
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

            if (dialogueData.dialogue[id].condition.Count > 0) //checks if there is any need to check the condition at all
            {
                //checks conditions to see if it can proceed or talk
                ChecksCondition();
                //return;
            }

            //clears text box and talks
            ClearAndTalk();
        }
        else //if it is, disable dialogue UI and manager, and sets id back to 0
        {
            npcInteract.canTalk = true;
            dialogueGrp.SetActive(false);
            id = 0;
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine(string txt)
    {
        if (!alreadyTyping)
        {
            alreadyTyping = true;
            teste++;
            dialogue.text = "";
            if (txt.Equals("second dialogue"))
                print($"rodou {teste} vezes. texto: {txt}");
            foreach (char c in txt.ToCharArray())
            {
                if (txt.Equals("second dialogue"))
                    print(c);
                dialogue.text += c;
                if (txt.Equals("second dialogue"))
                    print(dialogue.text);

                yield return new WaitForSeconds(speed);
            }

            if (dialogue.text == txt)
            {
                if (dialogueData.dialogue[id].choices.Count > 0 && choicesBox.activeSelf == false)
                {
                    ShowChoices();
                }
            }
            alreadyTyping = false;
        }
    }

    void ChecksCondition()
    {
        //checks conditions and acts accordingly
        foreach (Condition condTalk in dialogueData.dialogue[id].condition) //checks condition in the dialogue json
        {
            foreach (Condition condStatic in conditions) //checks static condition
            {
                if (condTalk.type.Equals(condStatic.type)) //if both dialogue and static condition types are the same...
                {
                    if (
                        condTalk.boolValue != condStatic.boolValue
                        || condTalk.number != condStatic.number
                    ) //but their other attributes aren't...
                    {
                        //print(id + " "+ dialogueData.dialogue[1].nextId);
                        if (id != dialogueData.dialogue[0].id)
                            NextLine(); //go to next line
                    }
                }
            }
        }

        //if either all of the conditions are satisfied or there is nothing to check, returns
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
        conditions = dialogueConditions.TurnToConditions(npcInteract.timesTalked <= 1);
        //gets id of the next dialogue accordingly to the choice
        id = choiceNextId;

        if (dialogueData.dialogue[id].condition.Count > 0) //if there is any need to check the condition, it does so
            ChecksCondition();

        //then, it destroy the choices and hides the choicesBox
        DestroyChoices();
        //writes in the text box about what you chose
        ClearAndTalk();
    }
    #endregion
}
