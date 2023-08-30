using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Artifacts
{
    SUMMER,
    AUTUMN,
    WINTER
}

public class CheckItemExistence : MonoBehaviour
{
    [SerializeField] Artifacts artifact;

    private void OnEnable()
    {
        switch (artifact)
        {
            case Artifacts.AUTUMN:
                if (DialogueConditions.hasAutumn)
                    Destroy(this.gameObject);
                break;

            case Artifacts.SUMMER:
                if (DialogueConditions.hasSummer)
                    Destroy(this.gameObject);
                break;

            case Artifacts.WINTER:
                if (DialogueConditions.hasWinter)
                    Destroy(this.gameObject);
                break;
        }
    }
}
