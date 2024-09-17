using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ships
{

public class ShipClass : MonoBehaviour
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
    public ComponentSlot armourSlot;
    public ShipSection[] sections;

}

}
