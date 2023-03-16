using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxLife;
    [SerializeField] int life;

    [SerializeField] int phyDefense;
    [SerializeField] int magDefense;

    [Header("Drop")]
    [SerializeField] GameObject[] dropItem;
    [Range(0, 100)]
    [SerializeField] float[] dropRate;

    private void Awake()
    {
        if(maxLife > 0) life = maxLife;

    }

    private void Update()
    {
        if (life <= 0)
            death();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "damage")
        {
            damage dmgScript = collision.gameObject.GetComponent<damage>();
            loseLife(dmgScript.dmg, dmgScript.dmgType);
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

    private void death()
    {
        GameObject chosenDrop = null;
        float lootDrop = Random.Range(0f, 100f);
        float auxRate = 100;

        for (int i = 0; i < dropItem.Length; i++) {
            if (lootDrop <= dropRate[i] && auxRate >= dropRate[i]) {
                auxRate = dropRate[i];
                chosenDrop = dropItem[i];

                print(dropItem[i].name);
            }
        }

        if (chosenDrop != null)
        {
            GameObject loot = Instantiate(chosenDrop, this.transform.position, Quaternion.identity);
            loot.GetComponent<drop>().launch(); 
        }
        Destroy(this.gameObject);
    }
}
