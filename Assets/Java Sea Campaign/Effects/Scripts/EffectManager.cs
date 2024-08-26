using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{


public class EffectManager : MonoBehaviour
{
    private static EffectManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static void SpawnEffect(Effect effect, Vector3 position)
    {
        _instance.InternalSpawnEffect(effect, position);
    }
    private void InternalSpawnEffect(Effect effect, Vector3 position)
    {
        // Find effect set that matches effect
        EffectSet set = null;

        GameObject obj = set.GetAvailableEffectObject();
    }

    
}

public class EffectSet
{
    public Effect[] _effectPool;

    public GameObject GetAvailableEffectObject()
    {
        // Find object from pool or instaniate it
        
        return null;
    }
}

public class Effect
{
    public GameObject prefab;
}

}