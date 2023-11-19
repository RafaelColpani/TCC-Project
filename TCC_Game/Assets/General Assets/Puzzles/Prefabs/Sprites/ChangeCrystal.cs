using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCrystal : MonoBehaviour
{
    [SerializeField] Color activeColor;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        if (rend == null)
            rend = GetComponent<Renderer>();

        rend.material.SetColor("_ActiveColor", activeColor);
        rend.material.SetInt("_IsActive", 0);
    }

    public void Activate(bool activation){
        rend.material.SetInt("_IsActive", activation?1:0);
    }
}
