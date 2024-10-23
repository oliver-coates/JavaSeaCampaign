using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


[CreateAssetMenu(fileName = "New Damage Type", menuName = "Java Asunder/Damage/Damage Type", order = 1)]
public class DamageType : ScriptableObject
{
    [Header("Decorative:")]
    [SerializeField] private string _descriptiveName;
    public string descriptiveName
    {
        get
        {
            return _descriptiveName;
        }	
    }


    [Header("Possible damage to integrity:")]
    [SerializeField] private float _minimumDamageToIntegrity;
    [SerializeField] private float _maximumDamageToIntegrity;
    public float GetDamageToIntegrity()
    {
        return Random.Range(_minimumDamageToIntegrity, _maximumDamageToIntegrity);
    }

    [Header("Effects caused by this damage type:")]
    [SerializeField] private DamageEffect[] _effectsCaused;
    public DamageEffect[] effectCauseds
    {
        get
        {
            return _effectsCaused;
        }
    }


    [Header("Possible starting intensity of damage type:")]
    [SerializeField] private float _minimumDamageStartingIntensity;
    [SerializeField] private float _maximumDamageStartingIntensity;


}

}