using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGate : MonoBehaviour
{
    [SerializeField]  float velocidade = 5.0f; // Velocidade de movimento do objeto
    [SerializeField]  float distancia = 5.0f; // Distância que o objeto se moverá

    private Vector3 startPosition;
    private int direcao = 1; // 1 para a direita, -1 para a esquerda

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move o objeto na direção atual
        transform.Translate(Vector3.right * direcao * velocidade * Time.deltaTime);

        // Verifica se o objeto atingiu a distância máxima na direção atual
        if (Mathf.Abs(transform.position.x - startPosition.x) >= distancia)
        {
            // Inverte a direção e move o objeto de volta
            direcao *= -1;
        }

        if (this.transform.position.x <= startPosition.x && direcao == -1)
            this.enabled = false;
    }

    private void OnDisable()
    {
        transform.position = startPosition;
        direcao = 1;
    }
}
