using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Java Asunder/Damage/Damage Effect", order = 1)]
public class DamageEffect : ScriptableObject
{
    [Header("Decorative")]
    [SerializeField] private string _descriptiveName;
    public string descriptiveName
    {
        get
        {
            return _descriptiveName;
        }	
    }


    [Header("Damage:")]
    // How much the section's integrity is reduced by 
    [SerializeField] private float _integrityReductionPerIntensity;
    public float integrityReductionPerIntensity
    {
        get
        {
            return _integrityReductionPerIntensity;
        }	
    }

    
}

}