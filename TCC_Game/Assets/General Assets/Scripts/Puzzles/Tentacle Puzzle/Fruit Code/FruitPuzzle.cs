using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using System.Linq;
using UnityEngine.PlayerLoop;

public class FruitPuzzle : MonoBehaviour
{
    [System.Serializable]
    public struct FruitDestination
    {
        [Tooltip("The fruit that is assigned with this destination")]
        public GameObject fruit;
        [Tooltip("The destination that is assigned with this fruit")]
        public GameObject destination;
        [Tooltip("The initial position of the correspondent fruit for restarting the challenge")]
        public Vector3 initialFruitPosition;
    }

    #region Inspector VARs
    [HeaderPlus(" ", "- FRUIT DESTINATION -", (int)HeaderPlusColor.green)]
    [Tooltip("The list of the fruit and its respesctive destination")]
    [SerializeField] FruitDestination[] fruitDestinations;

    [HeaderPlus(" ", "- CHALLENGE LOGIC -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The sequence to complete the puzzle, accordingly with the fruitDestinations indexes. " +
        "Must start with 0 and successively until arrays length - 1.")]
    [SerializeField] List<int> correctSequenceIndexes;

    [HeaderPlus(" ", "- CHALLENGE LOGIC -", (int)HeaderPlusColor.blue)]
    [SerializeField] bool _isActiveOnOff = false;
    [SerializeField] GameObject vfxObject;
    [SerializeField] GameObject activeObj;
    [SerializeField] GameObject desactiveObj;

    [HeaderPlus(" ", "- VFX WIN -", (int)HeaderPlusColor.blue)]
    [SerializeField] GameObject shaderVFX;
     [SerializeField] Vector3 VFXScale = new Vector3(32, 32, 0);
    [SerializeField] float VFXTime = 15f;
    private bool _VFXLerp = false;

    #endregion

    #region Private VARs
    private List<int> catchedSequence = new List<int>();

    private bool hasCompletedChallenge = false;

    public bool HasCompletedChallenge
    {
        get { return hasCompletedChallenge; }
        set { hasCompletedChallenge = value; }
    }
    #endregion
    private void Start()
    {
        shaderVFX.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if(_VFXLerp == true)
        {
            var VFXLerp = new Vector3(1, 1, 0);
            shaderVFX.transform.localScale = Vector3.Lerp(shaderVFX.transform.localScale, VFXScale, VFXTime * Time.deltaTime);
        }
    }

    #region Public Methods
    public void FruitReachedDestination(GameObject fruit) 
    {
        int matchedIndex = -1;

        for (int i = 0; i < fruitDestinations.Length; i++)
        {
            if (fruitDestinations[i].fruit == fruit)
            {
                matchedIndex = i;
                break;
            }

            if (i >= fruitDestinations.Length - 1)
            {
                Debug.LogError("Cannot find the correct fruit for the puzzle.");
                return;
            }
        }

        catchedSequence.Add(matchedIndex);
        if (catchedSequence.Count() >= correctSequenceIndexes.Count())
            VerifySequence();
    }

    public Transform GetCorrectDestination(GameObject fruit)
    {
        foreach (var fruitDestination in fruitDestinations)
        {
            if (fruitDestination.fruit == fruit)
                return fruitDestination.destination.transform;
        }

        return null;
    }

    public FruitDestination GetUniqueFruitDestination(GameObject fruit)
    {
        foreach (var fruitDestination in fruitDestinations)
        {
            if (fruitDestination.fruit == fruit)
                return fruitDestination;
        }


        return new FruitDestination();
    }
    #endregion

    #region Private Methods
    private void VerifySequence()
    {
        for (int i = 0; i < correctSequenceIndexes.Count(); i++)
        {
            if (catchedSequence[i] == correctSequenceIndexes[i]) continue;

            RestartChallenge();
            return;
        }

        CompletedChallenge();
    }

    /// <summary>Runs when player completes the puzzle succesfully</summary>
    private void CompletedChallenge()
    {
        hasCompletedChallenge = true;

        if(_isActiveOnOff == true)
        {
            _VFXLerp = true;

            var spriteRenderVFX = shaderVFX.GetComponent<SpriteRenderer>();
            spriteRenderVFX.material.SetFloat("_ActiveOnOff", 1);
            
            vfxObject.SetActive(true);
            activeObj.SetActive(true);

        }

        if(_isActiveOnOff == false)
        {
            vfxObject.SetActive(true);
            desactiveObj.SetActive(false);
        }
    }

    void RestartChallenge()
    {
        catchedSequence.Clear();

        foreach (var fruitDestination in fruitDestinations)
        {
            fruitDestination.fruit.transform.localPosition = fruitDestination.initialFruitPosition;
            fruitDestination.destination.SetActive(true);
            fruitDestination.fruit.SetActive(true);

            var objectRb = fruitDestination.fruit.GetComponent<Rigidbody2D>();

            objectRb.isKinematic = false;
            objectRb.gravityScale = 1;
            objectRb.mass = 1;
        }
    }
    #endregion

}
