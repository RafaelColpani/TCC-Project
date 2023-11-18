using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : IEnemyState
{
    #region Constructor VARs
    CharacterManager characterManager;
    EnemyCommands enemyCommands;
    EnemyBehaviour behaviour;
    List<IEnemyState> stateMachine;

    Transform body;
    Transform player;

    float maxPlayerDistance;
    #endregion

    #region Private VARs
    readonly string wanderingStateName = "Wandering";
    readonly string attackingStateName = "Attacking";
    readonly string deadStateName = "Dead";

    private bool isLockedInLeft = false;
    private bool isLockedInRight = false;
    #endregion

    public ChasingState(CharacterManager characterManager, EnemyCommands enemyCommands, EnemyBehaviour behaviour, List<IEnemyState> stateMachine, Transform body, Transform player, float maxPlayerDistance)
    {
        this.characterManager = characterManager;
        this.enemyCommands = enemyCommands;
        this.behaviour = behaviour;
        this.stateMachine = stateMachine;
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
        Debug.Log($"left: {isLockedInLeft} || right: {isLockedInRight}");

        //if (characterManager.IsFacingRight() && isLockedInLeft)
        //{
        //    isLockedInLeft = false;
        //    isLockedInRight = false;
        //}

        //else if (!characterManager.IsFacingRight() && isLockedInRight)
        //{
        //    isLockedInRight = false;
        //    isLockedInLeft = false;
        //}

        // travado olhando pra esquerda
        if (!characterManager.IsFacingRight())
        {
            if (isLockedInRight)
            {
                if (!enemyCommands.IsInEdge())
                {
                    isLockedInRight = false;
                }

                return;
            }

            else if (enemyCommands.IsInEdge())
                isLockedInLeft = true;
        }

        // travado olhando pra direita
        else if (characterManager.IsFacingRight())
        {
            if (isLockedInLeft)
            {
                if (!enemyCommands.IsInEdge())
                {
                    isLockedInLeft = false;
                }

                return;
            }

            else if (enemyCommands.IsInEdge())
                isLockedInRight = true;
        }

        if (!enemyCommands.IsInEdge())
        {
            isLockedInRight = false;
            isLockedInLeft = false;
        }

        ////////////

        if (Mathf.Abs(player.position.x - body.position.x) <= 0.1f)
        {
            enemyCommands.StopWalk();
        }

        else if (WalkValue() == 1 && !isLockedInRight)
        {
            enemyCommands.ContinueWalkRight();
        }

        else if (WalkValue() == -1 && !isLockedInLeft)
        {
            enemyCommands.ContinueWalkLeft();
        }

        else
            enemyCommands.StopWalk();
    }

    public IEnemyState ChangeState()
    {
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

    private bool ReachedMaxDistance()
    {
        return enemyCommands.IsInEdge() ||
               //enemyCommands.GetExitPatrolRegion() ||
               (body.position - player.position).sqrMagnitude >= maxPlayerDistance;
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
