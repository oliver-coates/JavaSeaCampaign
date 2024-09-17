using System;
using System.Collections;
using System.Collections.Generic;
using KahuInteractive.HassleFreeSaveLoad;
using UnityEngine;

namespace Ships
{

[Serializable]
public class Ship : SerializedObject
{
    public ShipClass shipClass;
    public Nation nation;

    [Header("Decorative:")]
    public string shipName = "Unnnamed Ship";
    

    public string GetFullName()
    {
        return "PREFIX " + shipName;
        //return nation.shipPrefix + " " + shipName;
    }
}

}