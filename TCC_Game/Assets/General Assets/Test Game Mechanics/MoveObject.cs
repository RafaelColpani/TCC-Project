using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public string tagDoPlayer = "Player";
    public float velocidadeMovimento = 5f;

    private bool playerEstaPerto = false;
    private Transform playerTransform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagDoPlayer))
        {
            playerEstaPerto = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tagDoPlayer))
        {
            playerEstaPerto = false;
        }
    }

    private void Update()
    {
        if (playerEstaPerto)
        {
            // Calcula a direção do movimento.
            Vector3 direcaoMovimento = playerTransform.position - transform.position;
            direcaoMovimento.Normalize();

            // Move o objeto na direção do jogador.
            transform.Translate(direcaoMovimento * velocidadeMovimento * Time.deltaTime);
        }
    }
}
