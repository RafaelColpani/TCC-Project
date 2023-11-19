using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuebraVidro : MonoBehaviour
{
    [SerializeField] GameObject crack;
    
    public void Crack(){
        crack.SetActive(true);
    }
}
