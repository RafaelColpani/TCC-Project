using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : IEnemyState
{
    EnemyCommands enemyCommands;
    EnemyBehaviour behaviour;
    List<IEnemyState> stateMachine;
    IsDamagedAndDead isDamagedAndDead;

    Transform body;
    Transform player;

    float minPlayerDistance;

    readonly string chasingStateName = "Chasing";
    readonly string deadStateName = "Dead";

    public WanderingState(EnemyCommands enemyCommands, EnemyBehaviour behaviour, List<IEnemyState> stateMachine, IsDamagedAndDead isDamagedAndDead, Transform body, Transform player, float maxPlayerDistance)
    {
        this.enemyCommands = enemyCommands;
        this.behaviour = behaviour;
        this.stateMachine = stateMachine;
        this.isDamagedAndDead = isDamagedAndDead;
        this.body = body;
        this.player = player;
        this.minPlayerDistance = maxPlayerDistance;
    }

    public void Enter()
    {
        enemyCommands.ContinueWalkRight();
        enemyCommands.WalkSpeedWandering();
    }

    public string StateName()
    {
        return "Wandering";
    }

    public void Update()
    {

    }

    public IEnemyState ChangeState()
    {
        if (!isDamagedAndDead.IsAlive)
            return GetState(deadStateName);

        else if ((this.body.position - this.player.position).sqrMagnitude <= this.minPlayerDistance)
            return GetState(chasingStateName);

        return null;
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
}
