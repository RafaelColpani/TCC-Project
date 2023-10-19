using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnim : MonoBehaviour
{
   [SerializeField]  string tagDoPlayer = "Player"; // A tag do objeto do jogador
    [SerializeField]  KeyCode teclaAtivacao = KeyCode.E; // A tecla para ativar a variável
    [SerializeField]  MonoBehaviour scriptParaAtivar; // Arraste o script no Inspector que contém a variável booleana

    private bool ativado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagDoPlayer))
        {
            ativado = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tagDoPlayer))
        {
            ativado = false;
        }
    }

    private void Update()
    {
        if (ativado && Input.GetKeyDown(teclaAtivacao) && scriptParaAtivar != null)
        {
            // Ative a variável booleana no script
            scriptParaAtivar.enabled = true;
        }
    }
}
