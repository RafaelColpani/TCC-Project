// Created by: Henrique Batista de Assis
// Date: 25/12/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingTests : MonoBehaviour
{
    TestInputs ti;
    [SerializeField] InputActionAsset actions;

#region Unity Methods
    private void Awake() 
    {
        ti = new TestInputs();

        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
            ti.LoadBindingOverridesFromJson(rebinds);

        ti.Test.Test_1.performed += _ => PerformedTest1();
        ti.Test.Test_2.performed += _ => PerformedTest2();
    }

    private void Update() 
    {
        var readedVector = ti.Test.Test_Vector2.ReadValue<Vector2>();
        Debug.Log(readedVector);
    }
#endregion

#region Input Events
public void PerformedTest1()
{
    Debug.Log("Testou 1");
}

public void PerformedTest2()
{
    Debug.Log("Testou 2");
}
#endregion

#region Enable & Disable
    private void OnEnable() 
    {
        ti.Enable();
    }

    private void OnDisable() 
    {
        ti.Disable();
    }
#endregion
}
