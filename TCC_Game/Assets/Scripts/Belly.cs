using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belly : MonoBehaviour
{
    [Header("BELLY TIMER")]
    [SerializeField] float bellyMaxTimer = 10;
    [SerializeField] float bellyTimer = 0;
    [Header("HP TIMER")]
    [SerializeField] float hpMaxTimer = 5;
    [SerializeField] float hpTimer = 0;
    [Header("HP RECOVER TIMER")]
    [SerializeField] float hpRecoverMaxTimer = 2;
    [SerializeField] float hpRecoverTimer;
    [Header("HP LOSS")]
    [SerializeField] int hpLossFromBelly = 2;

    Status stats;

    private void Awake()
    {
        stats = this.GetComponent<Status>();
        hpRecoverTimer = hpRecoverMaxTimer;
    }
    private void Update()
    {
        //gives time before hunger starts
        if (bellyTimer < bellyMaxTimer)
        {
            bellyTimer += Time.deltaTime;
        }
        else
        {
            BellyEmpty();
        }

        //if belly is full, but not hp, hp starts to regenerate
        if (stats.belly >= stats.maxBelly 
            &&
            stats.hp < stats.maxHp) 
        {
            BellyFull();
        }

        //if hp recovery timer is neither at limit, nor completely depleted, it slowly gets depleted to give time for hp to be depleted slowly
        if (hpRecoverTimer < hpRecoverMaxTimer
            &&
            hpRecoverTimer > 0)
        {
            hpRecoverTimer -= Time.deltaTime;
        }
    }

    void BellyEmpty()
    {
        if (stats.belly >= 0) //if belly is not empty, hunger depletes it
        {
            stats.belly -= Time.deltaTime;
        }
        else //if belly is empty
        {
            if (hpTimer < hpMaxTimer) //if timer doesn't reach limit, it increases
            {
                hpTimer += Time.deltaTime;
            }
            else //if timer reaches limit, it resets and hp is depleted
            {
                hpTimer = 0;
                stats.hp -= hpLossFromBelly;
            }
        }
    }

    void BellyFull() //what happens if belly is full
    {
        //hp increases only if timer of hp recovery is at limit
        if (hpRecoverTimer >= hpRecoverMaxTimer)
        {
            stats.hp += 1;
            hpRecoverTimer -= Time.deltaTime;
        }
        else if (hpRecoverTimer <= 0) //if timer of hp recovery reaches minimum, it gets fulfilled to the limit again
        {
            hpRecoverTimer = hpRecoverMaxTimer;
        }
    }

    public void Eat(int bellyFiller) // if character eats
    {
        //belly gets filled by how much callories the food gives it
        stats.belly += bellyFiller;
        //timer before hunger strikes is reset
        bellyTimer = 0;

        //if belly
        if (stats.belly >= stats.maxBelly)
            stats.belly = stats.maxBelly;

        
    }
}
