using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitHorizontalCreator : MonoBehaviour
{
    [SerializeField] GameObject fruitPrefab;
    public GameObject finalDestination;
    public Animator birdAnimator;
    [SerializeField] bool bolaAndamento = false;


    void Start()
    {
        if(!bolaAndamento)
            InvokeRepeating("SpawnFruits", 0f, 15f);
    }

    public void SpawnFruits()
    {
        Vector3 spawnPosition = new Vector3(
                Random.Range(-80f, -20f),
                Random.Range(-8f, -12f), 0);
        Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);

        bolaAndamento = true; 
    }

    public void CallAnimator()
    {
        print("animator chamado");
        birdAnimator.SetTrigger("Coletar");
    }

}
