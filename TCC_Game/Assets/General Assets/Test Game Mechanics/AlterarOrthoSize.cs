using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AlterarOrthoSize : MonoBehaviour
{
    public string tagDoJogador = "Player";
    public CinemachineVirtualCamera virtualCamera;
    public float novoOrthoSize = 8.92f; // O novo valor de Lens Ortho Size
    public float velocidadeTransicao = 0.8f; // Velocidade da transição (ajuste conforme necessário)

    private float orthoSizeOriginal; // O valor original de OrthographicSize
    private bool emTransicao = false; // Para controlar o estado de transição

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagDoJogador) && !emTransicao)
        {
            // Verifica se o objeto que passou possui a tag do jogador
            if (virtualCamera != null)
            {
                orthoSizeOriginal = virtualCamera.m_Lens.OrthographicSize;
                emTransicao = true;
            }
        }
    }

    private void Update()
    {
        if (emTransicao)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, novoOrthoSize, velocidadeTransicao * Time.deltaTime);

            // Quando a transição estiver próxima do valor desejado, finalize
            if (Mathf.Approximately(virtualCamera.m_Lens.OrthographicSize, novoOrthoSize))
            {
                emTransicao = false;
            }
        }
    }
}