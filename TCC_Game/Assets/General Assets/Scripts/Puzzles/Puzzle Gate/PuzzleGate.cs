using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PuzzleGate : MonoBehaviour
{
    [SerializeField][MinValue(0)] float velocidade = 5.0f; // Velocidade de movimento do objeto
    [SerializeField][MinValue(0)] float distancia = 5.0f; // Distância que o objeto se moverá
    [SerializeField] bool goToRight = true;

    private Vector3 startPosition;
    private int direcao; // 1 para a direita, -1 para a esquerda

    private void Start()
    {
        startPosition = transform.position;
        if (goToRight)
            direcao = 1;
        else
            direcao = -1;
    }

    private void Update()
    {
        // Move o objeto na direção atual
        transform.Translate(Vector3.right * direcao * velocidade * Time.deltaTime, Space.World);

        // Verifica se o objeto atingiu a distância máxima na direção atual
        if (Mathf.Abs(transform.position.x - startPosition.x) >= distancia)
        {
            // Inverte a direção e move o objeto de volta
            direcao *= -1;
        }

        if (this.transform.position.x <= startPosition.x && direcao == -1 && goToRight)
            this.enabled = false;
        else if (this.transform.position.x >= startPosition.x && direcao == 1 && !goToRight)
            this.enabled = false;
    }

    private void OnDisable()
    {
        transform.position = startPosition;

        if (goToRight)
            direcao = 1;
        else
            direcao = -1;
    }
}
