using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtGrid : MonoBehaviour
{
    [SerializeField] ChangeCam changecam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        changecam.changeCam(ChangeCam.CamType.FIXED, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        changecam.changeCam(ChangeCam.CamType.FIXED, false);
    }
}
