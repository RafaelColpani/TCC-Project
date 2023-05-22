using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : IEnemyState
{
    #region Constructor VARs
    EnemyCommands enemyCommands;
    EnemyBehaviour behaviour;
    List<IEnemyState> stateMachine;
    EnemyCollisionController enemyCollisionController;
    IsDamagedAndDead isDamagedAndDead;

    Transform body;
    Transform player;

    float maxPlayerDistance;
    #endregion

    #region Private VARs
    readonly string wanderingStateName = "Wandering";
    readonly string attackingStateName = "Attacking";
    readonly string deadStateName = "Dead";
    #endregion

    public ChasingState(EnemyCommands enemyCommands, EnemyBehaviour behaviour, List<IEnemyState> stateMachine, IsDamagedAndDead isDamagedAndDead, EnemyCollisionController enemyCollisionController, Transform body, Transform player, float maxPlayerDistance)
    {
        this.enemyCommands = enemyCommands;
        this.behaviour = behaviour;
        this.stateMachine = stateMachine;
        this.isDamagedAndDead = isDamagedAndDead;
        this.enemyCollisionController = enemyCollisionController;
        this.player = player;
        this.body = body;
        this.maxPlayerDistance = maxPlayerDistance;
    }

    #region Interface Methods
    public void Enter()
    {
        if (WalkValue() == 1)
            enemyCommands.ContinueWalkRight();

        else
            enemyCommands.ContinueWalkLeft();

        enemyCommands.WalkSpeedChasing();
    }

    public string StateName()
    {
        return "Chasing";
    }

    public void Update()
    {
        if (WalkValue() == 1)
            enemyCommands.ContinueWalkRight();

        else if (WalkValue() == -1)
            enemyCommands.ContinueWalkLeft();

        else
            enemyCommands.StopWalk();
    }

    public IEnemyState ChangeState()
    {
        if (!isDamagedAndDead.IsAlive)
            return GetState(deadStateName);

        else if (enemyCommands.IsInEdge() || (body.position - player.position).sqrMagnitude >= maxPlayerDistance)
            return GetState(wanderingStateName);

        else if (enemyCollisionController.TouchedPlayer)
            return GetState(attackingStateName);

        return null;
    }
    #endregion

    #region Private Methods
    private int WalkValue()
    {
        // player is at right
        if (player.position.x > body.position.x)
            return 1;

        // player is at left
        else if (player.position.x < body.position.x)
            return -1;

        // player is same x position
        return 0;
    }

    private IEnemyState GetState(string stateName)
    {
        foreach (IEnemyState state in stateMachine)
        {
            if (state.StateName() == stateName)
                return state;
        }

        return null;
    }
    #endregion
}
