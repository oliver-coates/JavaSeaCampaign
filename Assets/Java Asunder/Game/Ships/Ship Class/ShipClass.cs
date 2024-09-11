using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ships
{


[CreateAssetMenu(fileName = "UnnamedClass", menuName = "Ships/Ship Class", order = 1)]
public class ShipClass : ScriptableObject
{
    public string className;
    public ShipType shipType;
    public Nation defaultNation;

    [Header("Extra Stats:")]
    public int extra_manuverability;
    public int extra_armour;
    public int extra_repairs;
    public int extra_maxCondition;
    public int extra_cargo;

}

}
