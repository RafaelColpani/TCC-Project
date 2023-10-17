using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineOnInteract : MonoBehaviour
{
    [SerializeField] float activationSpeed;
    Renderer rend;
    bool isActive;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Update() {
        if (Input.GetMouseButtonDown(0)) 
        {
            print("trying to activate");
            isActive = !isActive;
            StartCoroutine(activation());
        }
    }
    IEnumerator activation() {
        if (isActive == true)
        {
            while (rend.material.GetFloat("_Floating") < 1)
            {
                rend.material.SetFloat("_Floating",
                    rend.material.GetFloat("_Floating") + Time.deltaTime * activationSpeed);
                yield return null;
            }
        }
        else 
        {
            while (rend.material.GetFloat("_Floating") > 0)
            {
                rend.material.SetFloat("_Floating",
                    rend.material.GetFloat("_Floating") - Time.deltaTime * activationSpeed);
                yield return null;
            }
        }

    }
}
