using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


[System.Serializable]
public class DamageInstance
{
    [Header("The type of damage this is (Flooding, Fire, etc)")]
    [SerializeField] private DamageEffect _damageEffect;
    public DamageEffect damageEffect
    {
        get
        {
            return _damageEffect;
        }	
    }
    

    [Header("The intensity of this damage:")]
    [Range(0f, 100f)]
    [SerializeField] private float _intensity;
    public float intensity
    {
        get
        {
            return _intensity;
        }	
    }


    public DamageInstance(DamageEffect type, float hitSeverity)
    {
        _damageEffect = type;
        _intensity = hitSeverity;
    }

    public void BoostDamage(float severity)
    {
        _intensity += severity;
    }

    public void Tick(float deltaTime)
    {
        _intensity += deltaTime * (_damageEffect.intensityGrowthBase + (_damageEffect.intensityGrowthPerIntensity * _intensity));
    }
 
}

}