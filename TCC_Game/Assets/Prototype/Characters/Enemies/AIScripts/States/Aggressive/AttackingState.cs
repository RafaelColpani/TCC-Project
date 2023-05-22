using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : IEnemyState
{
    #region Contructor VARs
    EnemyCommands enemyCommands;
    List<IEnemyState> stateMachine;

    float maxWaitTime;
    #endregion

    #region Private VARs
    float waitTimer = 0f;

    readonly string chasingStateName = "Chasing";
    #endregion

    public AttackingState(EnemyCommands enemyCommands, List<IEnemyState> stateMachine, float maxWaitTime)
    {
        this.enemyCommands = enemyCommands;
        this.stateMachine = stateMachine;
        this.maxWaitTime = maxWaitTime;
    }

    public void Enter()
    {
        enemyCommands.StopWalk();
    }

    public string StateName()
    {
        return "Attacking";
    }

    public void Update()
    {
        if (waitTimer >= maxWaitTime)
        {
            waitTimer = 0f;
            return;
        }

        waitTimer += Time.deltaTime;
    }

    public IEnemyState ChangeState()
    {
        if (waitTimer < maxWaitTime) return null;

        return GetState(chasingStateName);
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
