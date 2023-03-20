using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSstats : MonoBehaviour
{
    [SerializeField] float life, physAtk, magAtk, physDef, magDef;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("_life"))
        {
            PlayerPrefs.SetFloat("_life", life);
            PlayerPrefs.SetFloat("_physAtk", physAtk);
            PlayerPrefs.SetFloat("_magAtk", magAtk);
            PlayerPrefs.SetFloat("_physDef", physDef);
            PlayerPrefs.SetFloat("_magDef", magDef);
        }
    }

    public void changeStats(int moreLife, int morePhysAtk, int moreMagAtk, int morePhysDef, int moreMagDef) 
    {
        PlayerPrefs.SetFloat("_life", life + moreLife);
        PlayerPrefs.SetFloat("_physAtk", physAtk + morePhysAtk);
        PlayerPrefs.SetFloat("_magAtk", magAtk + moreMagAtk);
        PlayerPrefs.SetFloat("_physDef", physDef + morePhysDef);
        PlayerPrefs.SetFloat("_magDef", magDef + moreMagDef);
    }

}
