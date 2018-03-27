using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct sPosition
{
    public int x;
    public int y;
}

public class MainGameScene : MonoBehaviour
{
    public CameraObject PlayCameraObject;
    public MainGameUI GameUI;
    public TileMap _TileMap;

	void Start ()
    {
        Init();		
	}
	
	void Update ()
    {
        MessageSystem.Instance.ProcessMessage();
    }

    void Init()
    {
        _TileMap.Init();
        GameManager.Instance.SetMap(_TileMap);

        Character player;
        Character monster;

        player = CreateCharacter("Player", "character03");

        for (int i = 0; i < 31; i++)
            monster = CreateCharacter("Monster", "character04");

        player.BecomeViewer();

        // GameManager.Instance.TargetCharacter = monster;
    }

    Character CreateCharacter(string fileName, string resourceName)
    {
        string filePath = "Prefabs/CharacterFrame/Character";
        GameObject charPrefabs = Resources.Load<GameObject>(filePath);
        GameObject charGameObject = GameObject.Instantiate(charPrefabs);
        charGameObject.transform.SetParent(_TileMap.transform);
        charGameObject.transform.localPosition = Vector3.zero;

        Character character = charGameObject.GetComponent<Player>();
        switch(fileName)
        {
            case "Player":
                character = charGameObject.AddComponent<Player>();
                break;
            case "Monster":
                character = charGameObject.AddComponent<Monster>();
                break;
        }
        character.Init(resourceName);

        CreateGameSlider(character, true, Vector3.zero);
        CreateGameSlider(character, false, new Vector3(0.0f, 0.4f, 0.0f));

        return character;
    }

    void CreateGameSlider(Character character, bool isHPGuage, Vector3 position)
    {
        Slider slider = GameUI.CreateSlider(isHPGuage);
        character.LinkGameGuage(slider, position, isHPGuage);
    }
}
