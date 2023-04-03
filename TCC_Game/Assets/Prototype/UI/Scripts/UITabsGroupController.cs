using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabsGroupController : MonoBehaviour
{
    List<Toggle> toggles = new List<Toggle>();
    void Start()
    {
        Toggle[] tg = GetComponentsInChildren<Toggle>();

        int i=0;
        foreach(Toggle toggle in tg)
        {
            toggles.Add(toggle);
            Debug.Log(toggle.ToString());
            toggles[i++].isOn = false;
        }
        toggles[0].isOn = true;
    }


    void Update()
    {
        
    }
}
