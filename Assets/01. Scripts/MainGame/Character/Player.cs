using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Start ()
    {
	}

    override public void InitState()
    {
        base.InitState();

        {
            State state = new PathfindingIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new PathfindingImmediateState();
            state.Init(this);
            _stateMap[eStateType.PATHFINDING] = state;
        }
        {
            State state = new PathfindingMoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}
