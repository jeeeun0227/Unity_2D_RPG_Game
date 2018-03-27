using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSlider : GameSlider
{
    override public void Update()
    {
        _slider.value = _character.GetHP() / 100.0f;
    }
}
