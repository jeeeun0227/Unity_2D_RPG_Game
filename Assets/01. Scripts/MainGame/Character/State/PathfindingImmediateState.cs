using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingImmediateState : PathfindingState
{
    public override void Start()
    {
        base.Start();

        // 탐색
        while(0 != _pathfindingQueue.Count)
        {
            if (eUpdateState.BUILD_PATH == _updateState)
                break;
            UpdatePathfinding();
        }

        // 구축
        while(eStateType.MOVE != _nextState)
        {
            UpdateBuildPath();
        }
    }

    public override void Update()
    {
    }

    public override void Stop()
    {
        base.Stop();
    }
}
