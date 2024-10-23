using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "New Ship Class", menuName = "Java Asunder/Ships/Class", order = 0)]
public class ShipClassType : ScriptableObject
{
    
    public GameObject prefab;

    [Header("Additional")]
    public ShipType shipType;
    public float hullDisplacement;
    public float baseValue;
}

}