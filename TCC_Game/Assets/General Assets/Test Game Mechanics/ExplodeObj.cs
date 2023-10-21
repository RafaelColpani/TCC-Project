using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class ExplodeObj : MonoBehaviour
{
    private Explodable _explodable;

    void Start()
	{
		_explodable = GetComponent<Explodable>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Test"))
        {
            _explodable.explode();
		    ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
		    //ef.doExplosion(transform.position);
        }
    }
}
