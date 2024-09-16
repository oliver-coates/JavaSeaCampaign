using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ships
{


[CreateAssetMenu(fileName = "Unnamed Class", menuName = "Java Asunder/Ships/Ship Class", order = 1)]
public class ShipClass : ScriptableObject
{
    public string className;
    public ShipType shipType;
    public Nation manufacturerNation;
    public float displacement;
    public float value;


    [Header("Extra Stats:")]
    public int baseAgility;
    public int baseArmour;
    public int baseCargo;

    [Header("Section:")]
    public ShipSection[] sections;

}

}
