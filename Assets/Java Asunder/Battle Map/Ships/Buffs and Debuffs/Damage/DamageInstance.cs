using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


[System.Serializable]
public class DamageInstance
{
    public float integrityDamage
    {
        get
        {
            return (_intensity * _damageEffect.integrityReductionPerIntensity);
        }
    }

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

        _intensity = Mathf.Clamp(_intensity, 0f, 100f);
    }

    public void Tick(float deltaTime)
    {
        _intensity += deltaTime * (_damageEffect.intensityGrowthBase + (_damageEffect.intensityGrowthPerIntensity * _intensity));
 
        _intensity = Mathf.Clamp(_intensity, 0f, 100f);
    }
 
    public string GetDescription()
    {
        return _damageEffect.GetDescription(_intensity);
    }

}

}