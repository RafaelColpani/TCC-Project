using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class SpriteOpacity : MonoBehaviour
{
    [HeaderPlus(" ", "- Sprite Renderers -", (int)HeaderPlusColor.yellow)]
    [SerializeField] SpriteRenderer[] spriteRenderers; 

    [HeaderPlus(" ", "- Alpha -", (int)HeaderPlusColor.blue)]
    [SerializeField] [Range(0f, 10f)] float alpha = 2.5f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Obj na area do Alpha");

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if (spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = alpha;
                    spriteRenderer.color = color;
                }
            }
        }
    }
}
