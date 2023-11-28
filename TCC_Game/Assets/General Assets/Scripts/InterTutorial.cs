using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using KevinCastejon.MoreAttributes;

public class InterTutorial : MonoBehaviour
{
    [HeaderPlus(" ", "- TEXT -", (int)HeaderPlusColor.cyan)]
    [SerializeField] TextMeshProUGUI TMP;

    [HeaderPlus(" ", "- SCENE -", (int)HeaderPlusColor.cyan)]
    [SerializeField] string scene = "BasePosCut";

   
    void Start()
    {
        TMP.text = "< Tecla F >";
    }
    

}
