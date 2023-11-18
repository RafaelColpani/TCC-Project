using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCollectible : MonoBehaviour
{
    private SequentialPlatformPuzzle spp;
    private CollectibleGrid cg;

    private void Awake()
    {
        spp = GameObject.FindGameObjectWithTag("GridPuzzle").GetComponent<SequentialPlatformPuzzle>();
    }

    public void AddCollectibleGrid(CollectibleGrid cg)
    {
        this.cg = cg;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (!cg.CanPickUp) return;

        spp.CollectedCollectible(cg);
    }

    private void OnEnable()
    {
        if (spp.IsInSamePlatform(cg.gridPosition))
            cg.CanPickUp = true;
    }

    private void OnDisable()
    {
        cg.CanPickUp = false;
    }
}
