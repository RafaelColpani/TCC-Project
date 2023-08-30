using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    #region Inspector
    [SerializeField] Color damageColor;
    [SerializeField] GameObject spritesParent;
    [SerializeField] int numberOfFlashes;
    #endregion

    #region VARs
    private SpriteRenderer[] spriteRenderers;
    private IsDamagedAndDead isDamageAndDead;
    private Color originalColor;
    private Color colorToChange;
    private float totalDuration;
    private float flashDuration;
    private float timer;
    #endregion

    private void Start()
    {
        spriteRenderers = spritesParent.GetComponentsInChildren<SpriteRenderer>();
        isDamageAndDead = GetComponentInChildren<IsDamagedAndDead>();
        originalColor = spriteRenderers[0].color;
        totalDuration = isDamageAndDead.FixedInvincibilityTime;
        flashDuration = totalDuration / numberOfFlashes;
        colorToChange = damageColor;
    }

    private void Update()
    {
        if (isDamageAndDead.InvincibilityTime <= 0)
        {
            timer = 0;
            colorToChange = damageColor;
            foreach (SpriteRenderer sprite in spriteRenderers)
            {
                if (sprite.color != originalColor)
                {
                    sprite.color = originalColor;
                }
            }

            return;
        }
        
        UpdateColor();
    }

    private void UpdateColor()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / flashDuration);

        Color lerpedColor;

        if (colorToChange == damageColor)
            lerpedColor = Color.Lerp(originalColor, colorToChange, t);
        else
            lerpedColor = Color.Lerp(damageColor, colorToChange, t);

        foreach (SpriteRenderer sprite in spriteRenderers)
            sprite.color = lerpedColor;

        if (t >= 1)
        {
            timer = 0;

            if (colorToChange == damageColor)
                colorToChange = originalColor;
            else
                colorToChange = damageColor;
        }
    }
}
