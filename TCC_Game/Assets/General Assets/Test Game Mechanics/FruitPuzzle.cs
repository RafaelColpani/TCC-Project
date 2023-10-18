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

    [Header("GameObject")]
    public GameObject objetoParaAtivar;

    #endregion

    public  void AtivarProximaAlavanca() 
    {
        if (indiceSequencia < sequenciaCorreta.Length)
        {
            if (alavancas[indiceSequencia].name == sequenciaCorreta[indiceSequencia])
            {
                alavancas[indiceSequencia].SetActive(true);
                indiceSequencia++;

                if (indiceSequencia == sequenciaCorreta.Length)
                {
                    Debug.Log("Quebra-cabeça resolvido! As alavancas corretas foram ativadas.");
                    objetoParaAtivar.SetActive(!objetoParaAtivar.activeSelf);
                }
            }

            else
            {
                ReiniciarQuebraCabeca();
            }
        }
    }

    void ReiniciarQuebraCabeca()
    {
        
        indiceSequencia = 0;
        Debug.Log("Sequência errada! Reiniciando o quebra-cabeça.");
    }

}
