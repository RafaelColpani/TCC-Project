using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [HideInInspector] public enum DmgType { PHY, MAG }
    public DmgType dmgType = new DmgType();
    public int dmg;
}
