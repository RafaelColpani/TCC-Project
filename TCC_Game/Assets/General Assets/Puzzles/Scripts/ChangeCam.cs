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
    [SerializeField] InputHandler inputHandler;

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
        if (inputHandler != null)
        {
            inputHandler.canWalk = false;
            inputHandler.GetJumpCommand().SetCanJump(false);
        }

        yield return new WaitForSeconds(changeTime);

        cam.SetActive(false);
        playerCam.SetActive(true);
        if (inputHandler != null)
        {
            inputHandler.canWalk = true;
            inputHandler.GetJumpCommand().SetCanJump();
        }
    }
}
