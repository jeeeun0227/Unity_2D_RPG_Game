using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMoveState : State
{
    public override void Start()
    {
        base.Start();
        _character.PopPathfindingTileCell();

        _moveDuration = _moveSpeed;
    }

    public override void Stop()
    {
        base.Stop();
        _character.ClearPathfindingTileCell();
    }

    public override void Update()
    {
        if(false == _character.IsEmptyPathfindingTileCell())
        {
            if(_moveDuration < _moveSpeed)
            {
                _moveDuration += Time.deltaTime;
                return;
            }
            _moveDuration = 0.0f;

            TileCell tileCell = _character.PopPathfindingTileCell();

            sPosition curPosition;
            curPosition.x = _character.GetTileX();
            curPosition.y = _character.GetTileY();

            sPosition toPosition;
            toPosition.x = tileCell.GetTileX();
            toPosition.y = tileCell.GetTileY();

            eMoveDirection direction = GetDirection(toPosition, curPosition);
            _character.SetNextDirection(direction);

            if (false == _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY()))
            {
                if (_character.IsAttackable())
                    _nextState = eStateType.ATTACK;
                else
                    _nextState = eStateType.IDLE;
            }
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }


    // Move

    float _moveSpeed = 0.1f;
    float _moveDuration = 0.0f;

    eMoveDirection GetDirection(sPosition toPosition, sPosition curPosition)
    {
        if (toPosition.x < curPosition.x)
            return eMoveDirection.LEFT;
        if (curPosition.x < toPosition.x)
            return eMoveDirection.RIGHT;
        if (toPosition.y < curPosition.y)
            return eMoveDirection.DOWN;
        if (curPosition.y < toPosition.y)
            return eMoveDirection.UP;
        return eMoveDirection.DOWN;
    }
}
