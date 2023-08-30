using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEnabled : MonoBehaviour
{
    [SerializeField] GameObject uiSfxManager;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        //uiSfxManager.SetActive(true);
    }

    void Update()
    {
        
    }
}
