using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingState : State
{
    protected enum eUpdateState
    {
        PATHFINDING,
        BUILD_PATH,
    }
    protected eUpdateState _updateState = eUpdateState.PATHFINDING;

    public struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }
    protected List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();

    protected TileCell _goalTileCell;
    protected TileCell _reverseTileCell = null;

    override public void Start()
    {
        base.Start();

        _goalTileCell = _character.GetGoalTileCell();
        if(null != _goalTileCell)
        {
            GameManager.Instance.GetMap().ResetPathfinding();

            TileCell startTileCell =
                GameManager.Instance.GetMap().GetTileCell(_character.GetTileX(),
                                                    _character.GetTileY());

            sPathCommand command;
            command.tileCell = startTileCell;
            command.heuristic = 0;
            PushCommand(command);
        }
        else
        {
            _nextState = eStateType.IDLE;
        }

        _reverseTileCell = null;
        _updateState = eUpdateState.PATHFINDING;
    }

    public override void Stop()
    {
        base.Stop();

        _pathfindingQueue.Clear();
        _character.SetGoalTileCell(null);
    }

    override public void Update()
    {
        /*
        if (eStateType.NONE != _nextState)
        {
            _character.ChangeState(_nextState);
            return;
        }
        */
        switch(_updateState)
        {
            case eUpdateState.PATHFINDING:
                UpdatePathfinding();
                break;
            case eUpdateState.BUILD_PATH:
                UpdateBuildPath();
                break;
        }
    }

    protected void UpdatePathfinding()
    {
        // 길찾기 알고리즘이 시작
        if (0 != _pathfindingQueue.Count)
        {
            sPathCommand command = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);
            if (false == command.tileCell.IsPathfided())
            {
                command.tileCell.Pathfinded();

                // 목표에 도달 했나?
                if (command.tileCell.GetTileX() == _goalTileCell.GetTileX() &&
                    command.tileCell.GetTileY() == _goalTileCell.GetTileY())
                {
                    _reverseTileCell = command.tileCell;
                    _updateState = eUpdateState.BUILD_PATH;
                    return;
                }

                for (int direction = (int)eMoveDirection.LEFT;
                    direction < (int)eMoveDirection.DOWN + 1; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = command.tileCell.GetTileX();
                    curPosition.y = command.tileCell.GetTileY();
                    sPosition nextPosition = GetPositionByDirection(curPosition, (eMoveDirection)direction);

                    TileCell searchTileCell =
                        GameManager.Instance.GetMap().GetTileCell(nextPosition.x, nextPosition.y);
                    if (null != searchTileCell && searchTileCell.IsPathfindable() && false == searchTileCell.IsPathfided())
                    {
                        float distance = command.tileCell.GetDistanceFromStart() +
                            searchTileCell.GetDistanceWeight();

                        if(null == searchTileCell.GetPrevPathfindingCell())
                        {
                            searchTileCell.SetDistanceFromStart(distance);
                            searchTileCell.SetPrevPathfindingCell(command.tileCell);
                            //searchTileCell.SetPathfindingTestMark();

                            sPathCommand newCommand;
                            newCommand.tileCell = searchTileCell;
                            /*
                            newCommand.heuristic = CalcSimpleHeuristic(
                                                            command.tileCell,
                                                            searchTileCell,
                                                            _goalTileCell);
                            */
                            newCommand.heuristic = CalcAStarHeuristic(distance,
                                                            searchTileCell,
                                                            _goalTileCell);
                            PushCommand(newCommand);
                        }                
                        else
                        {
                            if(distance < searchTileCell.GetDistanceFromStart())
                            {
                                searchTileCell.SetDistanceFromStart(distance);
                                searchTileCell.SetPrevPathfindingCell(command.tileCell);

                                sPathCommand newCommand;
                                newCommand.tileCell = searchTileCell;
                                /*
                                newCommand.heuristic = CalcSimpleHeuristic(
                                                                    command.tileCell,
                                                                    searchTileCell,
                                                                    _goalTileCell);
                                */
                                newCommand.heuristic = CalcAStarHeuristic(distance,
                                                            searchTileCell,
                                                            _goalTileCell);
                                PushCommand(newCommand);
                            }
                        }
                    }
                }
            }
        }
    }

    protected void UpdateBuildPath()
    {
        if(null != _reverseTileCell)
        {
            _character.PushPathfindingTileCell(_reverseTileCell);

            //_reverseTileCell.ResetPathfindingTestMark();
            _reverseTileCell = _reverseTileCell.GetPrevPathfindingCell();
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
    }

    sPosition GetPositionByDirection(sPosition curPosition, eMoveDirection direction)
    {
        sPosition newPosition = curPosition;
        switch (direction)
        {
            case eMoveDirection.LEFT:
                newPosition.x--;
                break;
            case eMoveDirection.RIGHT:
                newPosition.x++;
                break;
            case eMoveDirection.UP:
                newPosition.y--;
                break;
            case eMoveDirection.DOWN:
                newPosition.y++;
                break;
        }
        return newPosition;
    }

    void PushCommand(sPathCommand command)
    {
        _pathfindingQueue.Add(command);

        // Sorting
        _pathfindingQueue.Sort(delegate (sPathCommand c1, sPathCommand c2)
        {
            if (c1.heuristic < c2.heuristic)
                return -1;
            if (c2.heuristic < c1.heuristic)
                return 1;
            return 0;
        });
    }

    float CalcSimpleHeuristic(TileCell tileCell, TileCell nextTileCell, TileCell goalTileCell)
    {
        float heuristic = 0.0f;

        int diffFromCurrent = 0;
        int diffFromNext = 0;

        // x 축
        {
            // 현재 타일부터 목표 까지의 거리
            diffFromCurrent = tileCell.GetTileX() - _goalTileCell.GetTileX();
            if (diffFromCurrent < 0)
                diffFromCurrent = -diffFromCurrent;

            // 검사할 타일부터 목표까지의 거리
            diffFromNext = nextTileCell.GetTileX() - _goalTileCell.GetTileX();
            if (diffFromNext < 0)
                diffFromNext = -diffFromNext;

            if (diffFromCurrent < diffFromNext)
                heuristic += 1.0f;
            else if (diffFromNext < diffFromCurrent)
                heuristic -= 1.0f;
        }

        // y 축
        {
            // 현재 타일부터 목표 까지의 거리
            diffFromCurrent = tileCell.GetTileY() - _goalTileCell.GetTileY();
            if (diffFromCurrent < 0)
                diffFromCurrent = -diffFromCurrent;

            // 검사할 타일부터 목표까지의 거리
            diffFromNext = nextTileCell.GetTileY() - _goalTileCell.GetTileY();
            if (diffFromNext < 0)
                diffFromNext = -diffFromNext;

            if (diffFromCurrent < diffFromNext)
                heuristic += 1.0f;
            else if (diffFromNext < diffFromCurrent)
                heuristic -= 1.0f;
        }

        return heuristic;
    }

    float CalcComplexHeuristic(TileCell nextTileCell, TileCell goalTileCell)
    {
        int distanceW = nextTileCell.GetTileX() - goalTileCell.GetTileX();
        int distanceH = nextTileCell.GetTileY() - goalTileCell.GetTileY();

        distanceW = distanceW * distanceW;
        distanceH = distanceH * distanceH;

        return (float)(distanceW + distanceH);
    }

    float CalcAStarHeuristic(float distanceFromStart, TileCell nextTileCell, TileCell goalTileCell)
    {
        return distanceFromStart + CalcComplexHeuristic(nextTileCell, goalTileCell);
    }
}
