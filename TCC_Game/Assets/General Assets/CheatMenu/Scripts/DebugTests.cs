using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTests : MonoBehaviour
{
    private void Start()
    {
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand<int>("value_test", "Aparece um print para testes de debug com um valor em Int", "value_test <value_amount>", (x) => { Debug.Log("Teste de debug, com value de: " + x); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand<int>("value_test", "Aparece um print para testes de debug com um valor em Int", "value_test <value_amount>", (x) => { Debug.Log("Teste de debug, com value de: " + x); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand<int>("value_test", "Aparece um print para testes de debug com um valor em Int", "value_test <value_amount>", (x) => { Debug.Log("Teste de debug, com value de: " + x); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand<int>("value_test", "Aparece um print para testes de debug com um valor em Int", "value_test <value_amount>", (x) => { Debug.Log("Teste de debug, com value de: " + x); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand("print_test", "Aparece um print para testes apenas", "print_test", () => { Debug.Log("Teste de debug"); });
        DebugController.AddCommand<int>("value_test", "Aparece um print para testes de debug com um valor em Int", "value_test <value_amount>", (x) => { Debug.Log("Teste de debug, com value de: " + x); });
    }
}
