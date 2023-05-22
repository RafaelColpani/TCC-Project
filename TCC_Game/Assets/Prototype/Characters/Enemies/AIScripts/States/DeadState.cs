using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IEnemyState
{
    EnemyCommands enemyCommands;

    public DeadState(EnemyCommands enemyCommands)
    {
        this.enemyCommands = enemyCommands;
    }

    public IEnemyState ChangeState()
    {
        return null;
    }

    public void Enter()
    {
        enemyCommands.StopWalk();
    }

    public string StateName()
    {
        return "Dead";
    }

    public void Update()
    {
        
    }
}
