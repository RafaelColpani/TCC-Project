using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using System.Linq;

[RequireComponent(typeof(ProceduralTorso))]
public class ChickenFruitFollow : MonoBehaviour
{
    #region Inspector VARs
    [HeaderPlus(" ", "- MOVE COMMAND -", (int)HeaderPlusColor.green)]
    [Tooltip("The speed that the chicken will run to get the fruit and return home")]
    [SerializeField] private float runSpeed;

    [HeaderPlus(" ", "- FRUIT -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The distance in sqrMagnitude to consider that chicken catched the fruit")]
    [SerializeField] private float catchFruitDistance;
    [Tooltip("The speed that will move the torso target to get the fruit")]
    [SerializeField] private float eatFruitSpeed;
    [Tooltip("The distance in sqrMagnitude to consider that chicken eated the fruit")]
    [SerializeField] private float eatFruitDistance;

    [HeaderPlus(" ", "- TORSO -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The speed that the target of the torso will return to initial position when eated a fruit")]
    [SerializeField] private float returnFromEatSpeed;

    [HeaderPlus(" ", "- PUZZLE -", (int)HeaderPlusColor.cyan)]
    [Tooltip("The script of the fruit puzzle")]
    [SerializeField] FruitPuzzle fruitPuzzle;
    #endregion

    #region Private VARs
    private MoveCommand moveCommand;
    private ProceduralLegs proceduralLegs;
    private CharacterManager characterManager;
    private ProceduralTorso proceduralTorso;

    private Vector3 homePosition;
    private Vector3 initialTorsoTargetPosition;
    private Queue<GameObject> fruitObjs = new Queue<GameObject>();

    private bool isGoingToFruit = false;
    private bool isReturningHome = false;
    private bool isEatingFruit = false;

    private float eatTimer = 2;
    private float eatTimerCounter = 0;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        proceduralLegs = GetComponent<ProceduralLegs>();
        characterManager = GetComponent<CharacterManager>();
        proceduralTorso = GetComponent<ProceduralTorso>();
        moveCommand = new MoveCommand(proceduralLegs, runSpeed * characterManager.DirectionMultiplier());

        homePosition = this.transform.position;
    }

    private void Start()
    {
        initialTorsoTargetPosition = proceduralTorso.GetTarget().localPosition;
    }

    private void FixedUpdate()
    {
        MoveFlow();

        if (isGoingToFruit)
            CatchFruit();
        else if (isReturningHome)
            ReturnHome();
        else if (isEatingFruit)
        {
            EatFruit();
            EatTimer();
        }
    }
    #endregion

    #region Private Methods
    private void MoveFlow()
    {
        if (!isGoingToFruit && !isReturningHome)
            moveCommand.Execute(this.transform, GetRunValue(0));

        else
        {
            Vector3 destination = homePosition;

            if (isGoingToFruit)
                destination = fruitObjs.Peek().transform.position;

            moveCommand.Execute(this.transform, GetRunValue(destination.x));
        }
    }

    private void CatchFruit()
    {
        isReturningHome = false;
        proceduralTorso.MoveTarget(initialTorsoTargetPosition, returnFromEatSpeed);
        var distance = (fruitObjs.Peek().transform.position - this.transform.position).sqrMagnitude;
        if (distance <= catchFruitDistance)
        {
            isGoingToFruit = false;
            isEatingFruit = true;
        }
    }

    private void EatFruit()
    {
        proceduralTorso.CanMoveTarget = false;
        var fruitRb = fruitObjs.Peek().GetComponent<Rigidbody2D>();
        fruitRb.isKinematic = true;
        fruitRb.velocity = Vector2.zero;

        var distance = (fruitObjs.Peek().transform.position - proceduralTorso.GetTarget().position).sqrMagnitude;

        if (distance > eatFruitDistance && eatTimerCounter < eatTimer)
        {
            var relativePosition = transform.InverseTransformPoint(fruitObjs.Peek().transform.position);
            proceduralTorso.MoveTarget(relativePosition, eatFruitSpeed);
        }

        // fruit is eated here
        else
        {
            var fruit = fruitObjs.Dequeue();
            fruit.SetActive(false);
            isEatingFruit = false;

            // TODO: VFX of eated fruit here

            if (fruitObjs.Count() <= 0)
            {
                isReturningHome = true;
                proceduralTorso.CanMoveTarget = true;
            }

            else
            {
                isGoingToFruit = true;
            }

            eatTimerCounter = 0;
            //respawn fruit
            RespawnFruit(fruitPuzzle.GetUniqueFruitDestination(fruit));
        }
    }

    private void EatTimer()
    {
        eatTimerCounter += Time.fixedDeltaTime;
    }

    private void ReturnHome()
    {
        proceduralTorso.MoveTarget(initialTorsoTargetPosition, returnFromEatSpeed);
        var distance = (homePosition - this.transform.position).sqrMagnitude;
        if (distance <= 0.001f)
        {
            isReturningHome = false;
        }
    }

    private float GetRunValue(float xDestination)
    {
        if (!isGoingToFruit && !isReturningHome)
            return 0;

        else if (this.transform.position.x >= xDestination)
            return -1;

        else
            return 1;
    }

    private void RespawnFruit(FruitPuzzle.FruitDestination fruitDestination)
    {
        if (fruitDestination.fruit == null) { Debug.LogError("Fruit not found for respawn."); return; }

        var fruitRb = fruitDestination.fruit.GetComponent<Rigidbody2D>();
        fruitRb.gravityScale = 1;
        fruitRb.mass = 1;
        fruitRb.isKinematic = false;

        fruitDestination.fruit.transform.position = fruitDestination.initialFruitPosition;
        fruitDestination.fruit.SetActive(true);
    }
    #endregion

    #region Public Methods
    public void TriggerFruitFollow(GameObject fruit)
    {
        isGoingToFruit = true;
        fruitObjs.Enqueue(fruit);
    }

    public MoveCommand GetMoveCommand()
    {
        return this.moveCommand;
    }
    #endregion
}
