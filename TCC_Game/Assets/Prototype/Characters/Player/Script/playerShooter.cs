using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShooter : MonoBehaviour
{
    Quaternion rot;
    [SerializeField] GameObject physBullet, magBullet;
    [SerializeField] Transform obj;
    List<GameObject> objList;

    private void Start()
    {
        objList = new List<GameObject>();
        ObtainChildren(obj.transform);
    }

    void ObtainChildren(Transform children) 
    {
        foreach (Transform objChild in children) 
        {
            objList.Add(objChild.gameObject);
            if (objChild.childCount > 0)
            {
                ObtainChildren(objChild.transform);
            }
        }
    }

    void Update()
    {
        if (PauseController.GetIsPaused()) return;
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
        blt.GetComponent<damage>().creator = objList;

        print($"bullet: {blt!=null} " + " " + "[ playerShooter.cs ]");
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
