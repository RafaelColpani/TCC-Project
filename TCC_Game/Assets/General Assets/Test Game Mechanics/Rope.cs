using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public float gravityScale, mass;
    Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        gravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
        mass = player.GetComponent<Rigidbody2D>().mass;

        print($"gravityScale: {gravityScale}, mass {mass}");
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.gameObject.GetComponent<Rigidbody2D>().mass = 0;

            player.GetComponent<MoveTest>().isOnRope = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject collisionGO = collision.gameObject;

        if (collisionGO == player.gameObject)
        {
            float movimentoVertical = Input.GetAxis("Vertical");
            Vector3 direcaoMovimento = new Vector3(0, movimentoVertical,0);
            player.position += direcaoMovimento * 1.8f *Time.deltaTime;

            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        player.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        player.GetComponent<Rigidbody2D>().mass = mass;

        player.GetComponent<MoveTest>().isOnRope = false;
    }

}
