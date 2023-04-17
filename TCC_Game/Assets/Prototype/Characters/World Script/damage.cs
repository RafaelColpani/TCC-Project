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
    Rigidbody rb;
    [HideInInspector] public List<GameObject> creator;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.AddForce(transform.right * 5, ForceMode.Impulse);


        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //se o colisor não for o criador da bala, ela n será destroída
        if (!creator.Contains(collision.gameObject))
        {
            print("destrói bala: "+collision.gameObject.name);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //se o colisor não for o criador da bala, ela n será destroída
        if (!creator.Contains(collision.gameObject))
        {
            print("destrói bala: " + collision.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
