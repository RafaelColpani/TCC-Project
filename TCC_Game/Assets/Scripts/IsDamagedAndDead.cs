using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
//using Random = UnityEngine.Random;
using KevinCastejon.MoreAttributes;

public class IsDamagedAndDead : MonoBehaviour
{
    #region calibration
    /*[HeaderPlus(" ", "STATS", (int)HeaderPlusColor.cyan)]
    [SerializeField] int maxLife;
    [SerializeField] int life;

    [SerializeField] int phyDefense;
    [SerializeField] int magDefense;*/

    [HeaderPlus(" ", "DEATH", (int)HeaderPlusColor.magenta)]
    [SerializeField] ParticleSystem deathParticles;

    [HeaderPlus(" ", "DROP", (int)HeaderPlusColor.green)]
    [SerializeField] GameObject interactable;
    //[SerializeField] bool isPlayer = false;
    [SerializeField] List<GameObject> dropItem;
    [SerializeField] List<float> dropRate;
    #endregion

    #region variables
    Vector2 damageOrigin;
    [SerializeField]bool isAlive = true;
    Status stats;
    #endregion

    private void Awake()
    {
        //if(stats.maxHp > 0) stats.hp = stats.maxHp;

        List<float> ordenedDropRates = new List<float>(dropRate);
        ordenedDropRates.Sort();
        stats = GetComponent<Status>();
        //Array.Copy(dropRate, ordenedDropRates, dropRate.Count);
        //Array.Sort(ordenedDropRates);
    }

    private void Update()
    {
        if (stats.hp <= 0 && isAlive)
        {
            Death();
            isAlive = false;
        }
    }

    #region damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("damage") && isAlive)
        {
            damage dmgScript = collision.gameObject.GetComponent<damage>();

            //se dano for do player, mas o alvo nao for player, ou se dano nao for gerado pelo player, o alvo deve perder vida
            /*if (
                (dmgScript.isPlayer && !this.gameObject.CompareTag("Player"))
                ||
                (!dmgScript.isPlayer)
                )
            {
                loseLife(dmgScript.dmg, dmgScript.dmgType);
            }*/

            //se o dano for criado pelo ataque do objeto X, o mesmo não deverá levar o dano
            if(dmgScript.creator != this.gameObject)
            {
                loseLife(dmgScript.dmg, dmgScript.dmgType);
            }
        }
    }

    private void loseLife (int damage, damage.DmgType dmgType)
    {
        int lifeLoss;

        if(dmgType == global::damage.DmgType.PHY)
            lifeLoss = damage - stats.phyDefense;
        else
            lifeLoss = damage - stats.magDefense;

        if (lifeLoss <= 0) 
            lifeLoss = 1;

        stats.hp -= lifeLoss;
    }
    #endregion
    #region death
    private void Death()
    {
        if (!this.CompareTag("Player"))
        {
            interactable.SetActive(true);
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            DropInventory();
            Destroy(this.gameObject);
        } 
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

    public void RemoveDrop(GameObject removedItem) 
    {
        dropRate.RemoveAt(dropItem.IndexOf(removedItem));
        dropItem.Remove(removedItem);
    }

    public void Drop() {
        GameObject chosenDrop = null;
        float lootDrop = Random.Range(0f, 100f);

        List<float> ordenedDropRates = new List<float>(dropRate);
        ordenedDropRates.Sort();

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

        Destroy(this.gameObject);
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
