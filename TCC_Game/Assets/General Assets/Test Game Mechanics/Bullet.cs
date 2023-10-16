using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform pontoDeTiro; // O ponto de origem do disparo.
    public GameObject projetilPrefab; // Prefab do projetil.
    public float forcaDoTiro = 10f; // Força do disparo.
    public float taxaDeTiro = 0.5f; // Taxa de disparo (intervalo entre os tiros).
    public float tempoDeVidaDoTiro = 3f; // Tempo de vida do tiro em segundos.

    private float tempoUltimoTiro;

    private void Start()
    {
        tempoUltimoTiro = Time.time;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= tempoUltimoTiro + taxaDeTiro)
        {
            Atirar();
        }
    }

    private void Atirar()
    {
        tempoUltimoTiro = Time.time;

        // Cria um novo projetil.
        GameObject projetil = Instantiate(projetilPrefab, pontoDeTiro.position, pontoDeTiro.rotation);

        // Obtém o componente Rigidbody2D do projetil.
        Rigidbody2D rbProjetil = projetil.GetComponent<Rigidbody2D>();

        // Aplica uma força ao projetil para criar a trajetória parabólica.
        if (rbProjetil != null)
        {
            Vector2 direcaoDoTiro = pontoDeTiro.right; // Direção do tiro.
            rbProjetil.velocity = direcaoDoTiro * forcaDoTiro;

            // Define o tempo de vida do tiro.
            Destroy(projetil, tempoDeVidaDoTiro);
        }
    }
}
