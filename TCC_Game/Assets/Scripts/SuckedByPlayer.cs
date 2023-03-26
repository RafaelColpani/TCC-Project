using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckedByPlayer : MonoBehaviour
{
    #region callibration
    [Header("Circular Raycast")]
    [SerializeField] float radius;
    [SerializeField] float distance;
    [Header("Approaching Player")]
    [SerializeField] float speed = 1f;
    [Tooltip("curve of velocity for when player approaches")]
    [SerializeField] AnimationCurve velocityCurve;
    [SerializeField] float destroyDistance = 0.5f;
    #endregion

    #region variables
    Rigidbody2D rb;
    #endregion

    private void Awake()
    {
        rb =  this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, distance);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Circle cast hit object with 'Player' tag!");
                GameObject player = hit.collider.gameObject;
                goToPlayer(player.GetComponent<Life>(), player.GetComponent<Transform>().position);
            }
        }

        //DebugExtension.DebugCircle(transform.position, radius, Color.blue);
    }

    private void goToPlayer(Life playerInventory, Vector3 playerPos) 
    {
        Vector2 direction = playerPos - transform.position;

        float distance = direction.magnitude;

        if (distance <= destroyDistance)
            getToInventory(playerInventory);

        float speedMultiplier = velocityCurve.Evaluate(distance);

        Vector2 velocity = direction.normalized * speed * speedMultiplier;
        
        
        transform.Translate(velocity * Time.deltaTime);
    }

    private void getToInventory(Life playerInventory) 
    {
        //makes object get to the inventory and then destroys the one on scene
        Destroy(this.gameObject);

    }

}
