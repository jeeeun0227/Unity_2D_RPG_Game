using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : State
{
    public override void Start()
    {
        base.Start();

        int damagePoint = _character.GetDamagePoint();
        _character.DecreaseHP(damagePoint);
        if( false == _character.IsLive())
        {
            _nextState = eStateType.DEATH;
        }
        else
            _nextState = eStateType.IDLE;
    }
}
