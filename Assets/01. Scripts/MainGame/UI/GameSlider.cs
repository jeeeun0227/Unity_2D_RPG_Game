using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSlider
{
    protected Character _character;
    protected Slider _slider;

    public void Init(Slider slider, Character character)
    {
        _character = character;
        _slider = slider;
    }

    virtual public void Update()
    {
    }
}
