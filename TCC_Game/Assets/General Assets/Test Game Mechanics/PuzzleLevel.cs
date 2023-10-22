using System.Collections;
using System.Collections.Generic;
using KevinCastejon.MoreAttributes;
using UnityEngine;

public class PuzzleLevel : MonoBehaviour
{
    #region Inspector VARS
    [HeaderPlus(" ", "- OBJECTIVE -", (int)HeaderPlusColor.green)]
    [Tooltip("The transform of the door that will open and its BELOW the other door.")]
    [SerializeField] Transform upperDoor;
    [Tooltip("The transform of the door that will open and its ABOVE the other door.")]
    [SerializeField] Transform lowerDoor;
    [Tooltip("The speed that the doors will open")]
    [SerializeField] float openSpeed = 5.0f;
    [Tooltip("The VFX that will reproduce when opened the door")]
    [SerializeField] GameObject vfxSmoke;

    [HeaderPlus(" ", "- AUX -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The timer of the cooldown to activate the totem again")]
    [SerializeField] float cooldownTimer = 1.0f;
    [SerializeField] bool singleActivationONOFF = true;
    #endregion

    #region Private VARS
    private readonly string playerTag = "Player";

    private bool activated = false;
    private bool singleActivation = false;
    private bool inCooldown = false;

    private int countActivationONOFF = 0;
    #endregion

    #region Public Methods
    public void ActivatedTotem()
    {
        if (!activated || singleActivation || inCooldown) return;

        // moves the lower door to down
        lowerDoor.Translate(Vector3.down * openSpeed * Time.deltaTime);
        vfxSmoke.SetActive(true);

        // moves the upper door to up
        upperDoor.Translate(Vector3.up * openSpeed * Time.deltaTime);

        if (singleActivationONOFF)
            singleActivation = true;

        else
        {
            countActivationONOFF++;
            IniciarCooldown();
        }

        if (countActivationONOFF > 1)
        {
            singleActivation = true;
            singleActivationONOFF = true;
        }
    }
    #endregion

    #region Private Methods
    private void IniciarCooldown()
    {
        inCooldown = true;
        StartCoroutine(EncerrarCooldown());
    }

    private IEnumerator EncerrarCooldown()
    {
        yield return new WaitForSeconds(cooldownTimer);
        inCooldown = false;
    }
    #endregion

    #region Unity Events
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag) || singleActivation) return;

        activated = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        activated = false;
        vfxSmoke.SetActive(false);
    }
    #endregion
}
