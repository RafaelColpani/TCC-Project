using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollectible : MonoBehaviour
{
    private SequentialPlatformPuzzle spp;

    private void Awake()
    {
        spp = GameObject.FindGameObjectWithTag("GridPuzzle").GetComponent<SequentialPlatformPuzzle>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        spp.CollectedCollectible();
        Destroy(this.gameObject);
    }
}
