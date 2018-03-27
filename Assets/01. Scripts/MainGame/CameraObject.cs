using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MapObject
{
    Vector3 _prevPosition = Vector3.zero;

    // Unity Functions

    void Start ()
    {
        _prevPosition = transform.position;
    }
	
	void Update ()
    {
        if(Input.GetMouseButton(2))
        {

        }
    }
}
