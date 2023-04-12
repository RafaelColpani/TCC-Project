using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    public bool isPlayer = false;

    [HideInInspector] public enum DmgType { PHY, MAG }
    public DmgType dmgType = new DmgType();
    public int dmg;
    [SerializeField] float lifeTime = 5;
    Rigidbody2D rb;
    [HideInInspector] public GameObject creator;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

        rb.velocity = Vector3.zero;
        rb.AddForce(transform.right * 5, ForceMode2D.Impulse);


        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //se o colisor não for o criador da bala, ela n será destroída
        if (creator != collision.gameObject)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se o colisor não for o criador da bala, ela n será destroída
        if (creator != collision.gameObject)
            Destroy(this.gameObject);
    }
}
