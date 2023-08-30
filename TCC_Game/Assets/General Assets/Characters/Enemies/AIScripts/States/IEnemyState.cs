using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public string StateName();
    public void Enter();
    public void Update();
    public IEnemyState ChangeState();
}