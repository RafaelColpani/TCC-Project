using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

public class FollowVFX : MonoBehaviour
{
    [HeaderPlus(" ", "- Follow Game Object -", (int)HeaderPlusColor.yellow)]
    [SerializeField] Transform Player; 
    [SerializeField] float followSpeed = 0.5f; 
    [SerializeField] float distanceY = 20.15f; 
    private Vector3 vfxPosInit;

    void Start()
    {
        vfxPosInit = transform.position;
    }

    void Update()
    {
        Vector3 vfxNewPos = transform.position;
        vfxNewPos.x = Mathf.Lerp(vfxNewPos.x, Player.position.x, followSpeed * Time.deltaTime);
        vfxNewPos.y = Mathf.Lerp(vfxNewPos.y, Player.position.y + distanceY, followSpeed * Time.deltaTime);

        vfxNewPos.z = vfxPosInit.z;

        transform.position = vfxNewPos;
    }
}
