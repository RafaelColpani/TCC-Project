using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectArtifact : MonoBehaviour
{
    enum ArtifactCase
    {
        SUMMER, AUTUMN, WINTER
    }

    [SerializeField] ArtifactCase biome;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        switch (biome)
        {
            case ArtifactCase.SUMMER:
                DialogueConditions.hasSummer = true;
                break;

            case ArtifactCase.AUTUMN:
                DialogueConditions.hasAutumn = true;
                break;

            case ArtifactCase.WINTER:
                DialogueConditions.hasWinter = true;
                break;
        }
    }
}
