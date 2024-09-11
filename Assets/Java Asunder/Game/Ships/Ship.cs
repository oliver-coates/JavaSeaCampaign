using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


[CreateAssetMenu(fileName = "UnnamedShip", menuName = "Ships/Ship", order = 1)]

public class Ship : ScriptableObject
{
    public ShipClass shipClass;
    public Nation nation;

    [Header("Decorative:")]
    public string shipName;
    public float displacement;
    public float value;

    public string GetFullName()
    {
        return nation.shipPrefix + " " + shipName;
    }
}

}