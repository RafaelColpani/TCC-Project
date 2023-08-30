using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class uiArtefatos : MonoBehaviour
{
    [Header("UI ToDo Text & GameObject ")] [Space(5)]
    [Tooltip("The name of text is > lbl < inside the component name ")] [SerializeField] TextMeshProUGUI summerText;
    [Tooltip("The name of text is > lbl < inside the component name ")] [SerializeField] TextMeshProUGUI autumnText;
    [Tooltip("The name of text is > lbl < inside the component name ")] [SerializeField] TextMeshProUGUI winterText;
    [SerializeField] GameObject baseText;
    [SerializeField] GameObject baseActiveR, baseActiveL;
    
    // Update is called once per frame
    void Start()
    {
        baseText.SetActive(false);
        baseActiveR.SetActive(false);
        baseActiveL.SetActive(false);
    }

    void Update()
    {
        if(DialogueConditions.hasSummer == true)
        {
            summerText.fontStyle = FontStyles.Strikethrough ;

            summerText.gameObject.transform.parent.GetComponent<Toggle>().isOn = true;
        }

        if(DialogueConditions.hasAutumn == true)
        {
            autumnText.fontStyle = FontStyles.Strikethrough ;

            autumnText.gameObject.transform.parent.GetComponent<Toggle>().isOn = true;
        }

        if(DialogueConditions.hasWinter == true)
        {
            winterText.fontStyle = FontStyles.Strikethrough ;

            winterText.gameObject.transform.parent.GetComponent<Toggle>().isOn = true;
        }

        if(DialogueConditions.hasSummer == true && DialogueConditions.hasAutumn == true && DialogueConditions.hasWinter == true)
        {
            baseText.SetActive(true);

            baseActiveR.SetActive(true);
            baseActiveL.SetActive(true);
            
        }
    }
}
