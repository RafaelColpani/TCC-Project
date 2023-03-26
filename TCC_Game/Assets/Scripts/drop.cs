using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class drop : MonoBehaviour
{
    #region calibration
    [HeaderPlus(" ", "INITIAL LAUNCHING", (int)HeaderPlusColor.cyan)]
    [Tooltip("force to launch to the air when spawning")]
    [SerializeField] float launchForce = 1;
    [Tooltip("curve of scaling when it pops up")]
    [SerializeField] AnimationCurve curve;
    #endregion

    #region variables
    Vector2 size;
    Rigidbody2D rb;
    Vector2 lastVel;
    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        size = this.transform.localScale;
        transform.localScale = Vector2.zero;

        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            PhysicsMaterial2D material = new PhysicsMaterial2D();
            material.bounciness = 0.3f;
            collider.sharedMaterial = material;
        }
    }

    private void Start()
    {
        StartCoroutine(scaleUp());
    }

    #region launching
    private IEnumerator scaleUp() {

        float scaleDuration = 0.3f;
        float scaleTimer = 0;

        while (scaleTimer < scaleDuration)
        {
            float t = scaleTimer / scaleDuration;
            float scale = curve.Evaluate(t);
            transform.localScale = size * scale;
            scaleTimer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = size;
    }

    public void launch(Vector2 distDamage) {
        //se loot vai na dire��o oposta do tiro
        /*float distSize = Mathf.Abs(distDamage.x) + Mathf.Abs(distDamage.y);
        Vector2 angle = new Vector2(distDamage.x / distSize * 1.5f, distDamage.y / distSize * 1.5f);

        angle += Vector2.up;

        rb.AddForce(angle.normalized * launchForce, ForceMode2D.Impulse);*/

        //se loot vai pra cima
        float angle = Random.Range(0.5f, 0.5f);
        rb.AddForce(new Vector2(angle, 1 - Mathf.Abs(angle)) * launchForce, ForceMode2D.Impulse);
    }
    #endregion
}
