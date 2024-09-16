using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


[CreateAssetMenu(fileName = "New Ship", menuName = "Java Asunder/Ships/Ship", order = 1)]

public class Ship : ScriptableObject
{
    public ShipClass shipClass;
    public Nation nation;

    [Header("Decorative:")]
    public string shipName;
    

    public string GetFullName()
    {
        return nation.shipPrefix + " " + shipName;
    }
}

}