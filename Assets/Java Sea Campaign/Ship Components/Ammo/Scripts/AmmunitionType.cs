using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AmmunitionType", menuName = "JavaSeaCampaign/AmmunitionType", order = 0)]
public class AmmunitionType : ScriptableObject 
{
    public GameObject _prefab;

    public float damage;
}