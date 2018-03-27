using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // Singleton
    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = new GameManager();
                _instance.Init();
            }
            return _instance;
        }
    }

    void Init()
    {
    }

    // Map

    TileMap _tileMap;

    public TileMap GetMap()
    {
        return _tileMap;
    }

    public void SetMap(TileMap map)
    {
        _tileMap = map;
    }

    // Test Target Chatacter

    public Character TargetCharacter;
}
