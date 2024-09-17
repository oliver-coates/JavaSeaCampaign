using System.Collections;
using System.Collections.Generic;
using KahuInteractive.HassleFreeSaveLoad;
using UnityEngine;

namespace Ships
{



public class Ship : SerializedObject
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