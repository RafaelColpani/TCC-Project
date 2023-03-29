using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("LIFE")]
    public int life = 1;
    public int maxHp = 10;
    public int hp;
    [Header("BELLY")]
    public float maxBelly = 10;
    public float belly;
    [Header("DEFENSES")]
    public int phyDefense = 0;
    public int magDefense = 0;

    private void Awake()
    {
        hp = maxHp;
        belly = maxBelly;
    }

}
