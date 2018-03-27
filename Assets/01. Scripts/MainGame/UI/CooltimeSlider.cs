using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooltimeSlider : GameSlider
{
    override public void Update()
    {
        _slider.value = _character.GetDeltaAttackCooltime() / _character.GetAttackCooltime();
    }
}
