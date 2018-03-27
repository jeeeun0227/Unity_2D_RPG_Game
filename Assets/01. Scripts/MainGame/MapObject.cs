using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMapObjectType
{
    NONE,
    MONSTER,
    TILE_OBJECT,
}

public class MapObject : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}


    // Info
    protected eMapObjectType _type = eMapObjectType.NONE;

    protected int _tileX = 0;
    protected int _tileY = 0;

    public eMapObjectType GetObjectType()
    {
        return _type;
    }


    public void SetPosition(Vector2 position)
    {
        gameObject.transform.localPosition = position;
    }

    virtual public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _tileLayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    public void BecomeViewer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, Camera.main.transform.localPosition.z);
    }

    public int GetTileX() { return _tileX; }
    public int GetTileY() { return _tileY; }

    public void SetTilePosition(int tileX, int tileY)
    {
        _tileX = tileX;
        _tileY = tileY;
    }

    // Layer

    protected eTileLayer _tileLayer;

    public eTileLayer GetCurrentLayer()
    {
        return _tileLayer;
    }

    // Move

    protected bool _canMove = true;

    public bool CanMove()
    {
        return _canMove;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }


    // Message

    virtual public void ReceiveObjectMessage(MessageParam msgParam)
    {
    }
}
