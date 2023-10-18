using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPuzzle : MonoBehaviour
{
    #region Variavel
    [Header("Seq")]
    public GameObject[] alavancas; // Array de GameObjects que representam as alavancas na cena
    public string[] sequenciaCorreta; // Sequência correta das alavancas a serem ativadas
    private int indiceSequencia = 0; // Índice atual na sequência

    #endregion

    void Start()
    {
        Debug.Log("Fazer a resolução do puzzle");
    }

    void AtivarProximaAlavanca() 
    {
        
    }


}
