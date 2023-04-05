using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("LIFE")]
    public int maxLife = 3;
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
        if (PlayerPrefs.HasKey("Player_Life"))
        {
            PlayerPrefs.SetInt("PlayerLife", maxLife);
            PlayerPrefs.SetInt("PlayerHP", maxHp);
            PlayerPrefs.SetFloat("PlayerBelly", maxBelly);
            PlayerPrefs.SetInt("Player_PhyDefense", phyDefense);
            PlayerPrefs.SetInt("Player_MagDefense", magDefense);
        }

        if(PlayerPrefs.HasKey("hasSummer")){
            PlayerPrefs.SetInt("hasSummer", 0);
            PlayerPrefs.SetInt("hasAutumn", 0);
            PlayerPrefs.SetInt("hasWinter", 0);
        }

        if (PlayerPrefs.HasKey("summerCondition"))
        {
            PlayerPrefs.SetInt("summerCondition", 1);
            PlayerPrefs.SetInt("autumnCondition", 1);
            PlayerPrefs.SetInt("winterCondition", 1);
        }

        life = maxLife;
        hp = maxHp;
        belly = maxBelly;
    }

}
