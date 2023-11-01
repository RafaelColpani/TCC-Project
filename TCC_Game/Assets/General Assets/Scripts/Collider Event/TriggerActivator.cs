using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject objectToActivate;
    [SerializeField] GameObject objectToDisable;
    [Space(10)]

    [Header("Bools")]
    [SerializeField] bool _isBullet = false;
    [SerializeField] bool _isDisable = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && _isBullet == true)
        {
            // Verifica se a tag da colisão é "Bullet".
            objectToActivate.SetActive(true); // Ativa o objeto quando a bala atinge o Trigger.
            gameObject.SetActive(false);

            if(_isDisable == true)
            {
                objectToDisable.SetActive(false);
            }
        }

        
    }
}
