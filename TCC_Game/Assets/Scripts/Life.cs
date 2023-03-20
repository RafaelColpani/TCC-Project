using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
//using Random = UnityEngine.Random;
using KevinCastejon.MoreAttributes;

public class Life : MonoBehaviour
{
    #region calibration
    [HeaderPlus(" ", "STATS", (int)HeaderPlusColor.cyan)]
    [SerializeField] int maxLife;
    [SerializeField] int life;

    [SerializeField] int phyDefense;
    [SerializeField] int magDefense;

    [HeaderPlus(" ", "DEATH", (int)HeaderPlusColor.magenta)]
    [SerializeField] ParticleSystem deathParticles;

    [HeaderPlus(" ", "DROP", (int)HeaderPlusColor.green)]
    [SerializeField] bool hasDrop = true;
    [SerializeField] List<GameObject> dropItem;
    [SerializeField] List<float> dropRate;
    #endregion

    #region variables
    Vector2 damageOrigin;
    #endregion

    private void Awake()
    {
        if(maxLife > 0) life = maxLife;

        List<float> ordenedDropRates = new List<float>(dropRate);
        ordenedDropRates.Sort();
        //Array.Copy(dropRate, ordenedDropRates, dropRate.Count);
        //Array.Sort(ordenedDropRates);

        for (int i = 0; i < 3; i++)
            print(ordenedDropRates[i]);
    }

    private void Update()
    {
        if (life <= 0)
            Death();
    }

    #region damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "damage")
        {
            damage dmgScript = collision.gameObject.GetComponent<damage>();
            loseLife(dmgScript.dmg, dmgScript.dmgType);

            if (life <= 0)
            {
                damageOrigin = this.transform.position - collision.gameObject.transform.position;
            }
        }
    }

    private void loseLife (int damage, damage.DmgType dmgType)
    {
        int lifeLoss;

        if(dmgType == global::damage.DmgType.PHY)
            lifeLoss = damage - phyDefense;
        else
            lifeLoss = damage - magDefense;

        if (lifeLoss <= 0) 
            lifeLoss = 1;

        life -= lifeLoss;
    }
    #endregion
    #region death
    private void Death()
    {
        if (hasDrop) Drop();
        else DropInventory();

        DeathParticles();
        Destroy(this.gameObject);
    }

    private void DeathParticles()
    {
        float distSize = Mathf.Abs(damageOrigin.x) + Mathf.Abs(damageOrigin.y);
        Vector2 angle = new Vector2(damageOrigin.x / distSize * 1.5f, damageOrigin.y / distSize * 1.5f);

        angle += Vector2.up;

        float targetAngle = Vector2.SignedAngle(Vector2.up, angle);

        ParticleSystem deathPrt = Instantiate(deathParticles, this.transform.position, Quaternion.Euler(180,90,0));
        ParticleSystem.ShapeModule prtShape = deathPrt.shape;
        prtShape.rotation = new Vector3(deathPrt.shape.rotation.x - targetAngle, 0, 0);
    }
    #endregion
    #region drop
    public void AddDrop(GameObject newItem) 
    {
        dropItem.Add(newItem);
        dropRate.Add(0);
    }

    private void Drop() {
        GameObject chosenDrop = null;
        float lootDrop = Random.Range(0f, 100f);

        List<float> ordenedDropRates = new List<float>(dropRate);
        ordenedDropRates.Sort();
        //Array.Copy(dropRate, ordenedDropRates, dropRate.Count);
        //Array.Sort(ordenedDropRates);

        for (int i = 0; i < 3; i++)
            print(ordenedDropRates[i]);

        for (int i = 0; i < dropRate.Count; i++)
        {
            for (int j = i + 1; j < ordenedDropRates.Count; j++)
            {
                if (dropRate[i] == ordenedDropRates[j])
                {
                    for (int h = j - 1; h >= 0; h--)
                    {
                        dropRate[i] += ordenedDropRates[h];
                    }
                    break;
                }
            }
        }

        float auxRate = 100;

        for (int i = 0; i < dropItem.Count; i++)
        {
            if (lootDrop <= dropRate[i] && auxRate >= dropRate[i])
            {
                auxRate = dropRate[i];
                chosenDrop = dropItem[i];
            }
        }

        if (chosenDrop != null)
        {
            GameObject loot = Instantiate(chosenDrop, this.transform.position, Quaternion.identity);
            loot.GetComponent<drop>().launch(damageOrigin);
        }
    }

    private void DropInventory() {
        for (int i = 0; i < dropItem.Count; i++)
        {
            GameObject loot = Instantiate(dropItem[i], this.transform.position, Quaternion.identity);
            loot.GetComponent<drop>().launch(damageOrigin);
        }
    }
    #endregion
}
