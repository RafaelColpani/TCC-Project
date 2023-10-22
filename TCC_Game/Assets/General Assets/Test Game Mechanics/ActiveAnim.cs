using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAnim : MonoBehaviour
{
    private readonly string tagDoPlayer = "Player"; // A tag do objeto do jogador

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
            scriptParaAtivar.enabled = false;
        }
    }

    public void ActivatedTotem()
    {
        print("ativado bergz");
        if (!ativado || scriptParaAtivar == null) return;
        print("ativado");

        scriptParaAtivar.enabled = true;
    }
}
