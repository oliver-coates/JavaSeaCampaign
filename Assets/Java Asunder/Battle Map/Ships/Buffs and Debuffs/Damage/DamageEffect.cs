using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Java Asunder/Damage/Damage Effect", order = 1)]
public class DamageEffect : ScriptableObject
{

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

    
    // Intensity growth per second
    [SerializeField] private float _intensityGrowthBase;
    public float intensityGrowthBase
    {
        get
        {
            return _intensityGrowthBase;
        }	
    }

    // Intensity growth per second multiplied by the intensity 
    [SerializeField] private float _intensityGrowthPerIntensity;
    public float intensityGrowthPerIntensity
    {
        get
        {
            return _intensityGrowthPerIntensity;
        }	
    }

    [Header("Decorative:")]
    [SerializeField] private string _damageName;
    [SerializeField] private string[] _intensityDescriptors;

    public string GetDescription(float intensity)
    {
        int intensityAsInt = (int) intensity;

        float intensityAsRange = intensityAsInt / 100f;

        int descriptorIndex = Mathf.FloorToInt(intensityAsRange * _intensityDescriptors.Length);
        

        return $"{_intensityDescriptors[descriptorIndex]} {_damageName} ({intensityAsInt}%)";
    }

}

}