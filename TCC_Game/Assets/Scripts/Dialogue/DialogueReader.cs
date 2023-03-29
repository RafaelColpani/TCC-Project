using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueReader : MonoBehaviour
{
    #region external variables
    [SerializeField] TextMeshProUGUI textComp;
    [SerializeField] string[] lines;
    [SerializeField] float speed;
    #endregion

    #region internal variables
    int index;
    #endregion

    private void Awake()
    {
        textComp.text = string.Empty;
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (textComp.text == lines[index])
            {
                NextLine();
            }
            else 
            { 
                StopAllCoroutines();
                textComp.text = lines[index];
            }
        }
    }

    void StartDialogue() 
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void NextLine() 
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComp.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else 
        { 
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine() 
    {
        foreach (char c in lines[index].ToCharArray()) 
        {
            textComp.text += c;

            yield return new WaitForSeconds(speed);
        }
    }
}
