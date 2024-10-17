using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "New Ammunition Type", menuName = "Java Asunder/Components/Ammunition", order = 0)]
public class AmmunitionType : ShipComponent 
{
    public GameObject _prefab;

    public float damage;
    public float velocity;
}

}