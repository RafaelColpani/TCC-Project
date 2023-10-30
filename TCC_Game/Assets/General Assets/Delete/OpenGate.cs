using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject objetoParaAtivar; // Referência ao GameObject que você deseja ativar
    public Collider2D triggerCollider; // Collider 2D Trigger que acionará a ativação
    public KeyCode teclaParaAtivar = KeyCode.E; // Tecla para ativar o GameObject

    private bool jogadorDentroDoCollider = false;

    void Update()
    {
        // Verifica se o jogador está dentro do Collider 2D Trigger e se a tecla "e" foi pressionada
        if (jogadorDentroDoCollider && Input.GetKeyDown(teclaParaAtivar))
        {
            AtivarOuDesativarGameObject();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorDentroDoCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorDentroDoCollider = false;
        }
    }

    void AtivarOuDesativarGameObject()
    {
        // Inverte o estado ativo/inativo do GameObject
        objetoParaAtivar.SetActive(!objetoParaAtivar.activeSelf);
    }

    private void OnDrawGizmos()
    {
        if (triggerCollider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(triggerCollider.bounds.center, triggerCollider.bounds.size);
        }
    }
}
