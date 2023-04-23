using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceived : MonoBehaviour
{
    #region Inspector Vars
    [SerializeField] float invincibilityTime;
    #endregion

    #region Vars
    private Status status;

    private float fixedinvincibilityTime;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        status = GetComponent<Status>();
        fixedinvincibilityTime = invincibilityTime;
        invincibilityTime = 0;
    }

    private void Update()
    {
        InvincibilityCounter();
    }
    #endregion

    #region Methods
    private void ReceiveDamage(int value = 1)
    {
        if (invincibilityTime > 0) return;

        status.hp -= value;
        invincibilityTime = fixedinvincibilityTime;
    }

    private void InvincibilityCounter()
    {
        if (invincibilityTime <= 0) return;

        invincibilityTime -= Time.deltaTime;
    }
    #endregion

    #region Unity Events
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("creature"))
        {
            ReceiveDamage();

            if (collision.GetComponentInParent<IASpider>() != null)
            {
                var iaSpider = collision.GetComponentInParent<IASpider>();
                iaSpider.Attacked();
            }
        }
    }
    #endregion
}
