using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public float velocidadeMovimento = 5f;
    public float forcaPulo = 10f;

    private Rigidbody2D rb2D;
    private bool estaNoChao;
    private float larguraDoSprite;

    public bool isOnRope = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        larguraDoSprite = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    private void Update()
    {
        // Verifique se o jogador está no chão.
        estaNoChao = Physics2D.Raycast(transform.position, Vector2.down, larguraDoSprite + 0.1f);

        // Movimento horizontal.
        float movimentoHorizontal = Input.GetAxis("Horizontal");
        Vector2 direcaoMovimento = new Vector2(movimentoHorizontal, 0);
        rb2D.velocity = new Vector2(direcaoMovimento.x * velocidadeMovimento, rb2D.velocity.y);

        // Pulo.
        if (estaNoChao && Input.GetButtonDown("Jump"))
        {
            if (!isOnRope)
                rb2D.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
        }
    }
}
