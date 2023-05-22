using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : IEnemyState
{
    EnemyCommands enemyCommands;
    EnemyBehaviour behaviour;
    List<IEnemyState> stateMachine;

    Transform body;
    Transform player;

    float minPlayerDistance;

    readonly string chasingStateName = "Chasing";

    public WanderingState(EnemyCommands enemyCommands, EnemyBehaviour behaviour, List<IEnemyState> stateMachine, Transform body, Transform player, float maxPlayerDistance)
    {
        this.enemyCommands = enemyCommands;
        this.behaviour = behaviour;
        this.stateMachine = stateMachine;
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
        if ((this.body.position - this.player.position).sqrMagnitude <= this.minPlayerDistance)
            return GetChaseState();

        return null;
    }

    private IEnemyState GetChaseState()
    {
        foreach (IEnemyState state in stateMachine)
        {
            if (state.StateName() == this.chasingStateName)
                return state;
        }

        return null;
    }
}
