using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    void Awake()
    {
        _type = eMapObjectType.MONSTER;
    }

    void Start()
    {
    }

    public override void InitState()
    {
        base.InitState();
        {
            State state = new MonsterIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}
