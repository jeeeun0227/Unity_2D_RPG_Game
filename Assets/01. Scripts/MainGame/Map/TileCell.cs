using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
    GROUND,
    MIDDLE,
    MAXCOUNT,
}

public class TileCell
{
    Vector2 _position;
    List<List<MapObject>> _mapObjectMap = new List<List<MapObject>>();

    int _tileX = 0;
    int _tileY = 0;

    public void Init(int x, int y)
    {
        _tileX = x;
        _tileY = y;

        for(int i=0; i<(int)eTileLayer.MAXCOUNT; i++)
        {
            List<MapObject> tileObjectList = new List<MapObject>();
            _mapObjectMap.Add(tileObjectList);
        }
    }

    public int GetTileX()
    {
        return _tileX;
    }

    public int GetTileY()
    {
        return _tileY;
    }

    public void SetPosition(float x, float y)
    {
        _position.x = x;
        _position.y = y;
    }

    public void AddObject(eTileLayer layer, MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)layer];

        int sortingOder = mapObjectList.Count;
        mapObject.SetSortingOrder(layer, sortingOder);
        mapObject.SetPosition(_position);
        mapObjectList.Add(mapObject);
    }

    public void RemoveOjbect(MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)mapObject.GetCurrentLayer()];
        mapObjectList.Remove(mapObject);
    }

    public List<MapObject> GetCollisionList()
    {
        List<MapObject> collisionList = new List<MapObject>();

        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> objectList = _mapObjectMap[layer];
            for (int i = 0; i < objectList.Count; i++)
            {
                if (false == objectList[i].CanMove())
                {
                    collisionList.Add(objectList[i]);
                }
            }
        }
        return collisionList;
    }

    public bool CanMove()
    {
        for(int layer=0; layer<(int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> objectList = _mapObjectMap[layer];
            for(int i=0; i< objectList.Count; i++)
            {
                if (false == objectList[i].CanMove())
                    return false;
            }
        }
        return true;
    }

    public bool IsPathfindable()
    {
        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> objectList = _mapObjectMap[layer];
            for (int i = 0; i < objectList.Count; i++)
            {
                if (eMapObjectType.MONSTER != objectList[i].GetObjectType() &&
                    false == objectList[i].CanMove())
                    return false;
            }
        }
        return true;
    }

    // 길찾기

    bool _isPathfinded = false;
    float _distanceFromStart = 0.0f;
    TileCell _prevTileCell;

    public void ResetPathfinding()
    {
        _isPathfinded = false;
        _distanceFromStart = 0.0f;
        _prevTileCell = null;
    }

    public void Pathfinded()
    {
        _isPathfinded = true;
    }

    public float GetDistanceFromStart()
    {
        return _distanceFromStart;
    }

    public void SetDistanceFromStart(float distance)
    {
        _distanceFromStart = distance;
    }

    public float GetDistanceWeight()
    {
        return 1.0f;
    }

    public void SetPrevPathfindingCell(TileCell tileCell)
    {
        _prevTileCell = tileCell;
    }

    public TileCell GetPrevPathfindingCell()
    {
        return _prevTileCell;
    }

    public bool IsPathfided()
    {
        return _isPathfinded;
    }

    public void SetPathfindingTestMark()
    {
        _mapObjectMap[(int)eTileLayer.GROUND][0].gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void ResetPathfindingTestMark()
    {
        _mapObjectMap[(int)eTileLayer.GROUND][0].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
