using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooter : MonoBehaviour
{
    Quaternion rot;
    [SerializeField] GameObject physBullet, magBullet;
    void Update()
    {
        TurnObject();

        if (physBullet != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(physBullet);
            }
        }
        if (magBullet != null)
        {
            if (Input.GetMouseButtonDown(1))
                Shoot(magBullet);
        }
    }

    private void TurnObject()
    {
        var mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        var mouseAng = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.AngleAxis(mouseAng, Vector3.forward);
        rot = this.transform.rotation;
    }

    public void Shoot(GameObject bullet)
    {
        var blt = Instantiate(bullet, transform.position, rot);
        blt.GetComponent<damage>().creator = this.gameObject;

        print($"bullet: {blt!=null}");
    }

    public void ChangeBullet(GameObject newBullet)
    {
        damage.DmgType dmgType = newBullet.GetComponent<damage>().dmgType;

        switch (dmgType)
        {
            case damage.DmgType.PHY:
                physBullet = newBullet;
                break;
            case damage.DmgType.MAG:
                magBullet = newBullet;
                break;
            default:
                break;
        }
    }
}
