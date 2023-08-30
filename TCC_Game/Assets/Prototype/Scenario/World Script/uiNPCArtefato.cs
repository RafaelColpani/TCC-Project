using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class uiNPCArtefato : MonoBehaviour
{
    [Header("UI ToDo Text & GameObject ")] [Space(5)]
    [Tooltip("The name of text is > lbl < inside the component name ")] [SerializeField] TextMeshProUGUI baseText;

    [SerializeField] GameObject gameObjectNPCText;
    [Tooltip("The name of text is > lbl < inside the component name ")] [SerializeField] TextMeshProUGUI NPCText;

    // Start is called before the first frame update
    void Start()
    {
        gameObjectNPCText.SetActive(false);
    }



    private void OnTriggerEnter2D(Collider2D collision) 
    {
        baseText.fontStyle = FontStyles.Strikethrough;
        baseText.gameObject.transform.parent.GetComponent<Toggle>().isOn = true;
        
        
        gameObjectNPCText.SetActive(true);


        

    
    }
}
