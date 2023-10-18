using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public bool _isBullet = false;
    public bool _isKey = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && _isBullet == true)
        {
            // Verifica se a tag da colisão é "Bullet".
            objectToActivate.SetActive(true); // Ativa o objeto quando a bala atinge o Trigger.
            gameObject.SetActive(false);
        }

        if (other.CompareTag("Player") && _isKey == true)
        {
            
            objectToActivate.SetActive(true);
            gameObject.SetActive(false);
          
        }
    }
}
