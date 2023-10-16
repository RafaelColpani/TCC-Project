using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoItens : MonoBehaviour
{
   public string tagFruit = "Test";
    public float velocidadeMovimento = 2.0f; // Velocidade de movimento dos inimigos
    public GameObject destinoPredefinido; // Objeto de destino predefinido

    private Transform target; // O objeto "Fruit" como alvo
    private bool emMovimento = false; // Para controlar o estado de movimento

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagFruit))
        {
            target = other.transform;
            emMovimento = true;
        }
    }

    private void Update()
    {
        if (emMovimento)
        {
            if (target != null)
            {
                Vector3 direcao = target.position - transform.position;
                direcao.Normalize();
                transform.position += direcao * velocidadeMovimento * Time.deltaTime;
            }
            else
            {
                // Se o alvo (Fruit) não existe mais, mova para o destino predefinido
                MoveToDestinoPredefinido();
            }
        }
    }

    private void MoveToDestinoPredefinido()
    {
        if (destinoPredefinido != null)
        {
            Vector3 direcaoDestino = destinoPredefinido.transform.position - transform.position;
            direcaoDestino.Normalize();
            transform.position += direcaoDestino * velocidadeMovimento * Time.deltaTime;
        }
    }
}
