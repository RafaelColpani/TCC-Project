using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// aaaaaa
/// </summary>
public class SuckedByPlayer : MonoBehaviour
{
    #region callibration
    [Header("Circular Raycast")]
    [SerializeField] float radius;
    [SerializeField] float distance;
    [Header("Approaching Player")]
    [SerializeField] float maxTimer = 3f;
    [SerializeField] float speed = 1f;
    [Tooltip("curve of velocity for when player approaches")]
    [SerializeField] AnimationCurve velocityCurve;
    [SerializeField] float destroyDistance = 0.5f;
    #endregion

    #region variables
    Rigidbody2D rb;
    float timer = 0;
    #endregion

    [SerializeField] Item item;
    [SerializeField] InventoryManager inventoryM;

    IsDamagedAndDead idadRef;

    private void Awake()
    {
        rb =  this.GetComponent<Rigidbody2D>();

        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().sprite = item.sprite;

        print("SuckedByPlayer AWAKE");
    }

    private void Start()
    {
        inventoryM = FindObjectOfType<InventoryManager>();

        this.GetComponent<SpriteRenderer>().color = Color.white;
        this.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }

    private void Update()
    {
        if (timer < maxTimer) 
        {
            timer += Time.deltaTime;
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, distance, LayerMask.GetMask("Player"));

        foreach (RaycastHit2D hit in hits)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

                GameObject player = hit.collider.gameObject;

                if (timer >= maxTimer)
                {
                    GoToPlayer(player.GetComponent<IsDamagedAndDead>(), player.GetComponent<Transform>().position);
                }
        }
        if (hits.Length == 0) rb.gravityScale = 1;
    }

    // Item é atraído ao player
    private void GoToPlayer(IsDamagedAndDead playerInventory, Vector3 playerPos) 
    {
        rb.gravityScale = 0;
        Vector2 direction = playerPos - transform.position;

        float distance = direction.magnitude;

        if (distance <= destroyDistance)
            GetToInventory(playerInventory);

        float speedMultiplier = velocityCurve.Evaluate(distance);

        Vector2 velocity = direction.normalized * speed * speedMultiplier;
        
        
        transform.Translate(velocity * Time.deltaTime);
    }

    private void GetToInventory(IsDamagedAndDead playerInventory) 
    {
        // makes object get to the inventory and then destroys the one on scene

        if (inventoryM.AddItem(item))
        {
            print($"Item {transform.name} ao inventario");
        }

        Destroy(this.gameObject);

    }

}
