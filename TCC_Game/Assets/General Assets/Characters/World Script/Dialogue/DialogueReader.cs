using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;
using Microsoft.Unity.VisualStudio.Editor;

[System.Serializable]
public class NameColor : SerializableDictionaryBase<string, Color> { }
[System.Serializable]
public class SeedType : SerializableDictionaryBase<string, Sprite> { }

public class DialogueReader : MonoBehaviour
{
    #region external variables
    public float speed = 0.5f;

    [SerializeField]
    GameObject dialogueGrp, nameGrp, gameOverGrp;
    [SerializeField]
    GameObject seedImg;
    public string fileName = "dialogue.json";

    [SerializeField]
    TMP_Text name, dialogue;

    [SerializeField]
    GameObject choicesBox;

    [SerializeField]
    GameObject choicesButton;

    [SerializeField] NameColor nameBoxTextColor;
    [SerializeField] NameColor dialogueBoxTextColor;
    [SerializeField] SeedType seedType;
    #endregion

    #region internal variables
    private DialogueData dialogueData;
    private int id;
    private int trueId;

    [HideInInspector]
    public NpcInteractable npcInteract;
    List<GameObject> buttons = new List<GameObject>();

    [HideInInspector]
    public Collider2D npcInteractCollider;
    DialogueConditions dialogueConditions = new DialogueConditions();
    List<Condition> conditions;
    bool alreadyTyping;
    bool metConditions = true;
    #endregion

    private int teste = 0;

    /// <summary>The function called when player execute the command to read dialogue.</summary>
    public void ExecuteCommmand()
    {
        if (Time.timeScale == 0) return;
        //if it is clicked, it checks whether the text in the dialogue box is the same as what is expected to be written
        //if it is, it goes to the next line. if not, it completes the dialogue and checks if it must show choices
        if (dialogue.text.Equals(dialogueData.dialogue[id].text))
        {
            if (choicesBox.activeSelf == false) //player can only skip if there isn't any choice box
            {
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

    #region lines
    int checkId(int nextId)
    {
        foreach (Dialogue dia in dialogueData.dialogue)
        {
            if (nextId == dia.id)
            {
                return dialogueData.dialogue.FindIndex(a => a.Equals(dia));
            }
        }
        return -1;
    }
    public void StartAll()
    {
        //finds path to the json file to be read
        string filePath = Path.Combine(Application.streamingAssetsPath, "JSON", fileName);

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

        //gets list of conditions
        conditions = dialogueConditions.TurnToConditions(NpcInteractable.timesTalked <= 1);
        //Starts dialogue
        StartDialogue();
    }

    void StartDialogue()
    {
        id = 0;
        //if it is not the first time player interacts with npc, it gets right to the choices
        if (NpcInteractable.timesTalked > 1)
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

    void NextLine()
    {
        if (id < dialogueData.dialogue.Count - 1) //checks if dialogues are not finished
        {
            //checks if there is a nextId
            if (dialogueData.dialogue[id].nextId == 0 || !metConditions) //if there isn't a nextId or if a condition is not met, id adds 1
            {
                id++;
            }
            else //if there is a nextId and there is no false condition, it gets the next id
            {
                id = checkId(dialogueData.dialogue[id].nextId);
            }
            if (!metConditions)
            {
                metConditions = true;
            }

            if (dialogueData.dialogue[id].id == 60 && gameOverGrp != null &&
            DialogueConditions.hasSummer &&
            DialogueConditions.hasAutumn &&
            DialogueConditions.hasWinter)
            {
                gameOverGrp.SetActive(true);
                PauseController.SetPauseAndTime(true);
            }

            if (dialogueData.dialogue[id].condition.Count > 0) //checks if there is any need to check the condition at all
            {
                //checks conditions to see if it can proceed or talk
                ChecksCondition();
            }

            if (!metConditions) return;
            //clears text box and talks
            ClearAndTalk();
        }
        else //if it is, disable dialogue UI and manager, and sets id back to 0
        {
            npcInteract.canTalk = true;
            dialogueGrp.SetActive(false);
            id = 0;
            gameObject.SetActive(false);
            PauseController.SetPause(false);
        }
    }
    #endregion

    #region typing
    void ClearAndTalk()
    {
        if (!alreadyTyping)
        {
            //if the dialogue was said by a character, show the name box
            if (dialogueData.dialogue[id].character != null)
            {
                name.text = dialogueData.dialogue[id].character;
                nameGrp.SetActive(true);
            }
            else //hides name box
            {
                nameGrp.SetActive(false);
            }

            dialogue.text = string.Empty;

            //color of name and text change accordingly to the name
            dialogue.color = dialogueBoxTextColor[name.text];
            if (nameGrp.activeSelf)
                this.name.color = nameBoxTextColor[name.text];

            //show seed
            seedImg.SetActive(false);
            if (!string.IsNullOrEmpty(dialogueData.dialogue[id].seed))
            {
                foreach (string key in seedType.Keys){
                    if(dialogueData.dialogue[id].seed.Equals(key)){
                        seedImg.SetActive(true);
                        seedImg.GetComponent<UnityEngine.UI.Image>().sprite = seedType[key];
                    }
                }
            }

            StartCoroutine(TypeLine(dialogueData.dialogue[id].text)); //types the text of the id's line}
        }
    }

    IEnumerator TypeLine(string txt)
    {
        if (!alreadyTyping)
        {
            alreadyTyping = true;
            teste++;
            dialogue.text = "";
            foreach (char c in txt.ToCharArray())
            {
                dialogue.text += c;

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
    #endregion

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
                        || condTalk.number != condStatic.number) // but their values arent't
                    {
                        //show that conditions arent met
                        metConditions = false;
                        NextLine();
                        return;
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
        conditions = dialogueConditions.TurnToConditions(NpcInteractable.timesTalked <= 1);
        //gets id of the next dialogue accordingly to the choice
        id = checkId(choiceNextId);

        if (dialogueData.dialogue[id].condition.Count > 0) //if there is any need to check the condition, it does so
            ChecksCondition();

        //then, it destroy the choices and hides the choicesBox
        DestroyChoices();
        //writes in the text box about what you chose
        ClearAndTalk();
    }
    #endregion
}
