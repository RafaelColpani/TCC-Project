using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
//using Random = UnityEngine.Random;
using KevinCastejon.MoreAttributes;

[RequireComponent(typeof(Status))]
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
    [SerializeField] GameObject deathIcon;
    [SerializeField] Vector3 deathIconOffset;
    [SerializeField] Transform iconFollowsThis;

    [HeaderPlus(" ", "INVINCIBILITY", (int)HeaderPlusColor.yellow)]
    [SerializeField] float invincibilityTime = 0;

    [HeaderPlus(" ", "DROP", (int)HeaderPlusColor.green)]
    [SerializeField] GameObject interactable; // raio de interação

    //[SerializeField] bool isPlayer = false;

    [SerializeField] List<GameObject> droppableItem; // item que sera dropado se o player morrer
    [SerializeField] List<float> dropRate; // taxa de drop

    #endregion

    #region variables
    Vector2 damageOrigin;
    [SerializeField] bool isAlive = true;
    Status stats;
    Belly belly;
    GameObject instantiatedDeathIcon;
    float fixedInvincibilityTime;

    public bool IsAlive
    {
        get { return isAlive; }
    }
    #endregion

    private void Awake()
    {
        //if(stats.maxHp > 0) stats.hp = stats.maxHp;
        if (this.gameObject.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("maxHP") > 0) stats.hp = PlayerPrefs.GetInt("maxHP");
        }

        List<float> ordenedDropRates = new List<float>(dropRate);
        ordenedDropRates.Sort();
        stats = GetComponent<Status>();
        belly = GetComponent<Belly>();
        fixedInvincibilityTime = invincibilityTime;
        invincibilityTime = 0;
    }

    private void Update()
    {
        if (PauseController.GetIsPaused()) return;
        if (isAlive)
        {
            if (stats.hp <= 0)
            {
                isAlive = false;
                Death();
            }
        }

        if (invincibilityTime <= 0) return;
        invincibilityTime -= Time.deltaTime;
    }

    #region damage
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PauseController.GetIsPaused()) return;
        if (!this.CompareTag("Player")) return;
        if (collision.CompareTag("damage") && isAlive && invincibilityTime <= 0 && collision.GetComponent<IsDamagedAndDead>().IsAlive)
        {
            invincibilityTime = fixedInvincibilityTime;
            damage dmgScript = collision.gameObject.GetComponent<damage>();

            if (!dmgScript.creator.Contains(collision.gameObject))
            {
                LoseLife(dmgScript.dmg, dmgScript.dmgType);
            }

            if (collision.GetComponentInParent<IASpider>() == null) return;
            collision.GetComponentInParent<IASpider>().Attacked();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.CompareTag("Player")) return;

        if (collision.CompareTag("damage") && isAlive && invincibilityTime <= 0)
        {
            invincibilityTime = fixedInvincibilityTime;
            damage dmgScript = collision.gameObject.GetComponent<damage>();

            if (!dmgScript.creator.Contains(collision.gameObject))
            {
                LoseLife(dmgScript.dmg, dmgScript.dmgType);
            }
        }
    }

    private void LoseLife(int damage, damage.DmgType dmgType)
    {
        int lifeLoss;

        if (dmgType == global::damage.DmgType.PHY)
        {
            lifeLoss = damage - stats.phyDefense;
        }
        else
        {
            lifeLoss = damage - stats.magDefense;
        }

        if (lifeLoss <= 0)
            lifeLoss = 1;

        if (this.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Player_HP", PlayerPrefs.GetInt("Player_HP") - lifeLoss);
        }

        stats.hp -= lifeLoss;
    }
    #endregion

    #region death

    /// <summary>
    /// Mata a criatura
    /// </summary>
    private void Death()
    {
        // SE NÃO FOR O PLAYER
        if (!this.CompareTag("Player"))
        {
            // habilita a area de interacao
            interactable.SetActive(true);
            //mostra ícone de morte acima da criatura
            instantiatedDeathIcon = Instantiate(deathIcon);
            if (iconFollowsThis == null)
                instantiatedDeathIcon.GetComponent<FollowObject>().obj = this.gameObject.transform;
            else instantiatedDeathIcon.GetComponent<FollowObject>().obj = iconFollowsThis;
            instantiatedDeathIcon.GetComponent<FollowObject>().offset = deathIconOffset;

            //obtem componente do gameobject "morto"
            //this.GetComponent<SpriteRenderer>().color = Color.black;
        }

        // SE FOR O PLAYER
        else
        {
            DropInventory();
            FindObjectOfType<InventoryManager>()?.RemoveAllItems();
            RespawnPlayer();
        }
    }
    #endregion

    // SUBSTITUIR OU INTEGRAR ESSA LÓGICA COM O FUNCIONAMENTO ATUAL DO INVENTARIO
    #region drop

    /// <summary>
    /// Adiciona item dropavel a lista de itens do jogador
    /// </summary>
    /// <param name="newItem"></param>
    public void AddDrop(GameObject newItem)
    {
        droppableItem.Add(newItem);
        dropRate.Add(0);
    }

    /// <summary>
    /// Remove item dropavel da lista
    /// </summary>
    /// <param name="newItem"></param>
    public void RemoveDrop(GameObject removedItem)
    {
        dropRate.RemoveAt(droppableItem.IndexOf(removedItem));
        droppableItem.Remove(removedItem);
    }

    /// <summary>
    /// Dropa os itens da lista
    /// </summary>
    public void Drop()
    {
        print("drop");
        GameObject chosenDrop = null;
        float lootDrop = Random.Range(0f, 100f);

        List<float> orderedDropRates = new List<float>(dropRate);
        orderedDropRates.Sort();

        for (int i = 0; i < dropRate.Count; i++)
        {
            for (int j = i + 1; j < orderedDropRates.Count; j++)
            {
                if (dropRate[i] == orderedDropRates[j])
                {
                    for (int h = j - 1; h >= 0; h--)
                    {
                        dropRate[i] += orderedDropRates[h];
                    }
                    break;
                }
            }
        }

        float auxRate = 100;

        for (int i = 0; i < droppableItem.Count; i++)
        {
            if (lootDrop <= dropRate[i] && auxRate >= dropRate[i])
            {
                auxRate = dropRate[i];
                chosenDrop = droppableItem[i];
            }
        }

        // [GABRIEL]
        //   OBS.: Se  "chosenDrop = null;" a condição abaixo nunca será atendida
        if (chosenDrop != null)
        {
            GameObject loot = Instantiate(chosenDrop, this.transform.position, Quaternion.identity);
            loot.GetComponent<drop>().launch();
        }

        // reminder: destroi este objeto, não o loot
        Destroy(instantiatedDeathIcon);
        Destroy(this.transform.root.gameObject);
    }

    /// <summary>
    /// Se o player morrer, dropa os itens do player para fora do inventário 
    /// </summary>
    /// 
    private void DropInventory()
    {
        for (int i = 0; i < droppableItem.Count; i++)
        {
            GameObject loot = Instantiate(droppableItem[i], this.transform.position, Quaternion.identity);
            loot.GetComponent<drop>().launch();
        }
    }

    private void RespawnPlayer()
    {
        var respawnPosition = new Vector3(0f, 5f, 0);

        var inputHandler = GetComponentInParent<InputHandler>();
        var proceduralLegs = GetComponentInParent<ProceduralLegs>();
        inputHandler.SetCanWalk();

        stats.hp = stats.maxHp;
        stats.life -= 1;
        stats.belly = stats.maxBelly;
        belly.ResetBellyTimer();
        proceduralLegs.ProceduralIsOn = false;
        isAlive = true;

        inputHandler.gameObject.transform.position = respawnPosition;
        this.transform.position = respawnPosition;

        proceduralLegs.ProceduralIsOn = true;
        inputHandler.SetCanWalk(true);
    }
    #endregion
}