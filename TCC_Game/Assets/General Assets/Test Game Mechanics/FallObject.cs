using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
   private bool deveCair = false;
    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0; // Inicialmente, desativamos a gravidade
    }

    private void Update()
    {
        if (deveCair)
        {
            rb2D.gravityScale = 1; // Ativamos a gravidade quando deveCair for verdadeiro
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            deveCair = true; // Quando o jogador colidir com o trigger, definimos deveCair como verdadeiro
        }
    }
}
