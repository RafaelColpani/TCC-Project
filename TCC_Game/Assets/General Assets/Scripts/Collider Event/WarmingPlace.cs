using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using KevinCastejon.MoreAttributes;

public class WarmingPlace : MonoBehaviour
{
    [HeaderPlus(" ", "- COLLIDER AREA -", (int)HeaderPlusColor.yellow)]
    [SerializeField] [Range(0, 15)] float radiusInspector = 5f;
    private CircleCollider2D _collider;
    private WarmingManager warmingManager;
    private readonly string warmingManagerName = "WarmingManager";

    private void Start()
    {
        warmingManager = GameObject.Find(warmingManagerName).GetComponent<WarmingManager>();
    }

    private void Update()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = radiusInspector;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        warmingManager.StayOnFire();
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        warmingManager.IsOnFire = false;
        Debug.LogWarning("Player esta fora da area de calor");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radiusInspector);
    }
}
