using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectivenessModifier
{
    public const float MODIFIER_TIME = 15f;

    public float effect;
    [SerializeField] private float _timeRemaining;
    public float timeRemaining
    {
        get
        {
            return _timeRemaining;
        }
    }

    public EffectivenessModifier(float effect)
    {
        this.effect = effect;
        _timeRemaining = MODIFIER_TIME;
    }

    public void Tick(float time)
    {
        _timeRemaining -= time;
    }

}
