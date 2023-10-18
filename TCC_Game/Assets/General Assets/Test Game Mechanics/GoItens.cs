using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoItens : MonoBehaviour
{
    #region Variaveis

    [Header("Tags")]
    [SerializeField] [Tooltip("Tag para o objeto que ira seguir")]        string tagFruit = "Test";
    [SerializeField] [Tooltip("Tag para o local que vai levar o objeto")] string tagDest = "Test2";
    [Space(10)]
    
    [Header("Configs Basicas")]
    [SerializeField] float velocidadeMovimento = 2.0f; // Velocidade de movimento dos inimigos
    [SerializeField] bool emMovimento = false;
    [Space(10)]

    [Header("Configs dos Objetos")]
    [SerializeField] [Tooltip("Dist do objeto que ira virar filho")] float disObject = 0.2f;
    [SerializeField] [Tooltip("Dist para destruir o obj filho")]     float distDestroy = 0.2f;
    [Space(10)]

    [Header("Target")]
    [SerializeField] Transform target; // O objeto "Fruit" como alvo
    [SerializeField] Rigidbody2D targetRigidbody; // Rigidbody2D do target
    [SerializeField] bool reativarRigidbody = false; // Para controlar se o Rigidbody2D deve ser reativado

    [Header("Destino")]
    [SerializeField] GameObject destinoPredefinido; // Objeto de destino predefinido com base na tagFruit
    #endregion
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagFruit))
        {
            target = other.transform;
            targetRigidbody = other.GetComponent<Rigidbody2D>();
            if (targetRigidbody != null)
            {
                // Desativar a simulação do Rigidbody2D
                targetRigidbody.sharedMaterial = null;
                targetRigidbody.simulated = false;
                reativarRigidbody = true;
            }

            target = other.transform;
            emMovimento = true;

            // Configura o destino predefinido com base na tagFruit
            destinoPredefinido = GameObject.FindGameObjectWithTag(tagDest);
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

                // Verifica a distância entre o objeto e o alvo
                float distancia = Vector3.Distance(transform.position, target.position);
                if (distancia < disObject) // Ajuste esse valor conforme necessário
                {
                    // Torna o objeto pai do target
                    target.SetParent(transform, true);

                    // Copia as propriedades do transform do pai (GoItens)
                    target.localPosition = Vector3.zero;
                    target.localRotation = Quaternion.identity;

                    if (reativarRigidbody && targetRigidbody != null)
                    {
                        // Reativa a simulação do Rigidbody2D
                        targetRigidbody.simulated = true;
                        reativarRigidbody = false;
                        targetRigidbody.mass = 0;
                        targetRigidbody.gravityScale = 0;

                    }
                }
            }

             if (transform.childCount > 0)
            {
                // Move para o destino predefinido
                Vector3 direcaoDestino = destinoPredefinido.transform.position - transform.position;
                direcaoDestino.Normalize();
                Debug.Log("Direcao Destino [ " + direcaoDestino + " ]");

                transform.position += direcaoDestino * velocidadeMovimento * Time.deltaTime;

                // Verifica a distância entre o objeto e o destino predefinido
                float distanciaDestino = Vector3.Distance(transform.position, destinoPredefinido.transform.position);
                Debug.Log("Dist Destino [ " + distanciaDestino + " ]");
                
                if (distanciaDestino < distDestroy) // Ajuste esse valor conforme necessário
                {
                    Destroy(target.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Desenha um gizmo de linha do objeto para o alvo
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }

        // Desenha um gizmo de linha do objeto para o destino predefinido
        if (destinoPredefinido != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, destinoPredefinido.transform.position);
        }
    }
}
