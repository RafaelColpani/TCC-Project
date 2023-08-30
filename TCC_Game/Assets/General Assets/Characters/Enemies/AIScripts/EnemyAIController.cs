using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using NaughtyAttributes;

public enum EnemyBehaviour
{
    AGGRESSIVE,
    COWARD,
    PACIFIC
}

[RequireComponent(typeof(EnemyCommands))]
[RequireComponent(typeof(CharacterManager))]
public class EnemyAIController : MonoBehaviour
{
    #region Inspector
    [HeaderPlus(" ", "- AI BEHAVIOUR -", (int)HeaderPlusColor.green)]
    [Tooltip("The behaviour that the enemy will have with the player. At the moment, only AGGRESSIVE is supported atm.")]
    [SerializeField] private EnemyBehaviour enemyBehaviour;

    [HeaderPlus(" ", "- AGGRESSIVE -", (int)HeaderPlusColor.yellow)]
    [Tooltip("The minimum square distance the enemy have to be to trigger the chasing State.")]
    [SerializeField] float minSqrPlayerDistance;
    [Tooltip("The maximum square distance the enemy have to be to trigger the wandering State.")]
    [SerializeField] float maxSqrPlayerDistance;
    [Tooltip("The time that enemy will wait until change its state from Attacking.")]
    [SerializeField] float attackedWaitTime;
    #endregion

    #region Private VARs
    CharacterManager characterManager;
    EnemyCommands enemyCommands;
    EnemyCollisionController enemyCollisionController;
    IsDamagedAndDead isDamagedAndDead;

    Transform playerBody;

    List<IEnemyState> stateMachine;
    IEnemyState currentState;

    private bool touchedPlayer = false;

    #region States Names
    readonly string wanderingStateName = "Wandering";
    readonly string chasingStateName = "Chasing";
    readonly string attackingStateName = "Attacking";
    #endregion
    #endregion

    #region Unity Methods
    private void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        enemyCommands = GetComponent<EnemyCommands>();
        enemyCollisionController = GetComponentInChildren<EnemyCollisionController>();
        isDamagedAndDead = GetComponentInChildren<IsDamagedAndDead>();
        var player = GameObject.Find("pfb_playerEntregaFinal");
        var playerRb = player.GetComponentInChildren<Rigidbody2D>();
        playerBody = playerRb.GetComponent<Transform>();

        AssignStates();
        currentState.Enter();
    }

    private void Update()
    {
        if (PauseController.GetIsPaused()) return;

        StatesFlow();
    }
    #endregion

    #region Private Methods
    private void AssignStates()
    {
        stateMachine = new List<IEnemyState>();

        switch (enemyBehaviour)
        {
            case EnemyBehaviour.AGGRESSIVE:
                var wanderingState = new WanderingState(enemyCommands, enemyBehaviour, stateMachine, isDamagedAndDead, characterManager.Body, playerBody, minSqrPlayerDistance);
                currentState = wanderingState;

                stateMachine.Add(wanderingState);
                stateMachine.Add(new ChasingState(enemyCommands, enemyBehaviour, stateMachine, isDamagedAndDead, enemyCollisionController, characterManager.Body, playerBody, maxSqrPlayerDistance));
                stateMachine.Add(new AttackingState(enemyCommands, stateMachine, isDamagedAndDead, attackedWaitTime));
                stateMachine.Add(new DeadState(enemyCommands));
                break;

            case EnemyBehaviour.COWARD:
                Debug.LogWarning($"Only AGGRESSIVE enemy behaviour is supported at the moment. Please, change it in the EnemyAIController in {this.gameObject.name} game object.");
                break;

            case EnemyBehaviour.PACIFIC:
                Debug.LogWarning($"Only AGGRESSIVE enemy behaviour is supported at the moment. Please, change it in the EnemyAIController in {this.gameObject.name} game object.");
                break;
        }
    }

    private void StatesFlow()
    {
        if (currentState.ChangeState() != null)
        {
            currentState = currentState.ChangeState();
            currentState.Enter();
        }

        currentState.Update();
    }
    #endregion

    #region Public Methods
    /// <summary>Gets if the enemy is in a platform edge.</summary>
    public bool IsInEdge()
    {
        return !JumpUtils.MoreThenHalfLegsIsGrounded(characterManager.GroundCheckParent, characterManager.GroundCheckDistance, characterManager.GroundLayers);
    }
    #endregion
}
