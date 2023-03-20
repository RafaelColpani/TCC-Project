using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] bool isPlayer = false;
    
    [HideInInspector] public enum DmgType { PHY, MAG }
    public DmgType dmgType = new DmgType();
    public int dmg;
    [SerializeField] float lifeTime = 5;
    Rigidbody2D rb;

    private void Awake()
    {
        if (isPlayer)
        {
            switch (dmgType) 
            { 
                case DmgType.MAG:
                    dmg += (int)PlayerPrefs.GetFloat("_magAtk");
                    break;
                case DmgType.PHY:
                    dmg += (int)PlayerPrefs.GetFloat("_physAtk");
                    break;
                default:
                    break;
            }
        }

        rb = this.GetComponent<Rigidbody2D>();

        rb.velocity = Vector3.zero;
        rb.AddForce( transform.right * 5, ForceMode2D.Impulse);


        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);   
    }
}
