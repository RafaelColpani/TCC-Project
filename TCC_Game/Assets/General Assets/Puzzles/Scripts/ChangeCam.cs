using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeCam : MonoBehaviour
{
    public enum CamType { FIXED, TEMP };
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject fixedCam;
    [SerializeField] GameObject tempCam;
    [SerializeField] float changeTime = 5;

    public void changeCam(CamType camtype, bool isIn)
    {

        switch (camtype)
        {
            case CamType.TEMP:
                playerCam.SetActive(false);
                tempCam.SetActive(true);
                StartCoroutine(Change(tempCam));
                break;
            case CamType.FIXED:
                if (isIn)
                {
                    playerCam.SetActive(false);
                    fixedCam.SetActive(true);
                }
                else
                {

                    playerCam.SetActive(true);
                    fixedCam.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    IEnumerator Change(GameObject cam)
    {
        yield return new WaitForSeconds(changeTime);
        cam.SetActive(false);
        playerCam.SetActive(true);
        yield return null;
    }
}
