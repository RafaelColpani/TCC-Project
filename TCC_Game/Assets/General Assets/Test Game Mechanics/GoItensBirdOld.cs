using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GoItensBirdOld : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField][Tooltip("Tag para a fruta que ira seguir")] static string tagFruit = "Test"; 
    [SerializeField][Tooltip("Tag para o local que vai levar a fruta")] static string tagDest = "Test2";
    [Space(10)]

    [Header("Configs Basicas")]
    [SerializeField] float velocidadeMovimento = 2.0f; // Velocidade de movimento dos inimigos
    [SerializeField] bool emMovimento = false;
    [Space(10)]

    [Header("Configs dos Objetos")]
    [SerializeField][Tooltip("Dist do objeto que ira virar filho")] float disObject = 0.2f;
    [SerializeField][Tooltip("Dist para destruir o obj filho")] float distDestroy = 0.2f;

    [Header("Target")]
    [SerializeField] Transform target; // O objeto "Fruit" como alvo
    [SerializeField] Rigidbody2D targetRigidbody; // Rigidbody2D do target
    [SerializeField] bool reativarRigidbody = false; // Para controlar se o Rigidbody2D deve ser reativado

    [Header("Destino")]
    [SerializeField] GameObject destinoPredefinido; // Objeto de destino predefinido com base na tagFruit

    [Header("Mobs transform")]
    [Tooltip("Bird, <insira mais se necessário>")]
    [SerializeField] Transform[] mobs = new Transform[1];

    [SerializeField] GameObject fruitPrefab;
    [SerializeField] GameObject returnPositionGO;

    [SerializeField] bool isBirdFruit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagFruit))
        {
            target = other.transform;
            targetRigidbody = other.GetComponent<Rigidbody2D>();

            if (!isBirdFruit && targetRigidbody != null)
            {
                // Desativar a simulação do Rigidbody2D
                targetRigidbody.sharedMaterial = null;
                targetRigidbody.simulated = false;
                //reativarRigidbody = true;
            }

            target = other.transform;
            emMovimento = true;
        }
    }


    void Start()
    {
        // Configura o destino predefinido com base na tagFruit
        returnPositionGO = new GameObject("Bird return point");
        returnPositionGO.transform.position = mobs[0].gameObject.transform.position;
    }


    void Update()
    {
        if (emMovimento)
        {
            if (target != null)
            {
                Vector3 direcao = target.position - mobs[0].transform.position;
                direcao.Normalize();
                mobs[0].transform.position += direcao * velocidadeMovimento * Time.deltaTime;

                // Verifica a distância entre o objeto e o alvo
                float distancia = Vector3.Distance(mobs[0].transform.position, target.position);

                if (distancia < disObject) // Ajuste esse valor conforme necessário ---- disObject
                {
                    // destroi o objeto
                    Destroy(target.gameObject);

                    if (!isBirdFruit && reativarRigidbody && targetRigidbody != null)
                    {
                        // Reativa a simulação do Rigidbody2D
                        targetRigidbody.simulated = true;
                        reativarRigidbody = false;
                        targetRigidbody.mass = 0;
                        targetRigidbody.gravityScale = 0;
                    }

                    // Move para o destino predefinido
                    Vector3 direcaoDestino = returnPositionGO.transform.position - mobs[0].transform.position;
                    direcaoDestino.Normalize();

                    mobs[0].transform.position += direcaoDestino * velocidadeMovimento * Time.deltaTime;

                    // Verifica a distância entre o objeto e o destino predefinido
                    float distanciaDestino = Vector3.Distance(mobs[0].transform.position, returnPositionGO.transform.position);


                    
                }

            }
            else
            {
                print("entrei no else");
                Vector3 direcaoDestino = returnPositionGO.transform.position - mobs[0].transform.position;
                direcaoDestino.Normalize();
                Debug.Log("Direcao Destino [ " + direcaoDestino + " ]");

                mobs[0].transform.position += direcaoDestino * velocidadeMovimento * Time.deltaTime;

                // Verifica a distância entre o objeto e o destino predefinido
                float distanciaDestino = Vector3.Distance(mobs[0].transform.position, returnPositionGO.transform.position);

                if (distanciaDestino < 0.2f)
                    emMovimento = false;
            }
        }

        else if (!target)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-80f, -20f),
                Random.Range(-8f, -12f), 0);

            Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
            
        }
    }

    private void OnDrawGizmos()
    {
        // Desenha um gizmo de linha do objeto para o alvo
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(mobs[0].transform.position, target.position);
        }

        // Desenha um gizmo de linha do objeto para o destino predefinido
        if (returnPositionGO != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(mobs[0].transform.position, returnPositionGO.transform.position);
        }
    }
}
