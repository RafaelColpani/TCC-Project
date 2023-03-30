using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DialogueReader2 : MonoBehaviour
{
    public float speed = 0.5f;
    [SerializeField] GameObject dialogueGrp;
    public string fileName = "dialogue.json";
    [SerializeField] TMP_Text name, dialogue;
    [SerializeField] GameObject choicesBox;
    [SerializeField] GameObject choicesButton;

    private DialogueData dialogueData;
    private int id;
    [HideInInspector]public NpcInteractable npcInteract;

    private void Start()
    {
        string filePath = Path.Combine(Application.dataPath,"JSON", fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            dialogueData = JsonUtility.FromJson<DialogueData>(dataAsJson);
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }

        StartDialogue();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (dialogue.text == dialogueData.dialogue[id].text)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogue.text = dialogueData.dialogue[id].text;

                if (dialogueData.dialogue[id].choices != null && choicesBox.activeSelf == false)
                {
                    ShowChoices();
                }
            }
        }
    }

    void StartDialogue()
    {
        if (npcInteract.timesTalked <= 1)
        {
            id = 0;
            print("first time dialogue");
        }
        else 
        {
            id = 2;
            print("second+ time dialogue");
        }

        StartCoroutine(TypeLine(dialogueData.dialogue[id].text));
        if (dialogueData.dialogue[id].choices != null) 
        {
            ShowChoices();
        }
    }
   
    void NextLine()
    {
        if (id < dialogueData.dialogue.Count - 1)
        {
            if (/*string.IsNullOrEmpty(dialogueData.dialogue[id].nextId.ToString())*/
                dialogueData.dialogue[id].nextId == 0
                )
            {
                id++;
                print($"next line hasn't a next id: {id}");
            }
            else 
            {
                id = dialogueData.dialogue[id].nextId;
                print($"next line has a next id: {id}");
            }
            name.text = dialogueData.dialogue[id].character;
            dialogue.text = string.Empty;
            StartCoroutine(TypeLine(dialogueData.dialogue[id].text));
        }
        else
        {
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
            if (dialogueData.dialogue[id].choices != null && choicesBox.activeSelf == false)
            {
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
            ch.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            ch.GetComponent<DialogueChoice>().nextId = choice.nextId;
        }
    }

    public void DestroyChoices() 
    {
        foreach (Transform child in choicesBox.GetComponentsInChildren<Transform>()) 
        {
            Destroy(child);
        }
        choicesBox.SetActive(false);
    }

    public void Chose(int choiceId) 
    {
        id = dialogueData.dialogue[id].choices[choiceId].nextId;
    }
    #endregion
}
