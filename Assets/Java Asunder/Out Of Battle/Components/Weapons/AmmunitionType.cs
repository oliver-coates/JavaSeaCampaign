using System.Collections;
using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "New Ammunition Type", menuName = "Java Asunder/Components/Ammunition", order = 0)]
public class AmmunitionType : ShipComponent 
{
    public GameObject _prefab;

    public float damage;
    public float velocity;

    [Header("Decoration:")]
    public EffectType splashEffect;
    public EffectType explosionEffect;


}



}