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
    private const string DEFAULT_NAME  = "Unnnamed Ship";

    [SerializeField] private ShipClassType _shipClass;
    public ShipClassType shipClass
    {
        get
        {
            return _shipClass;
        }	
    }

    [SerializeField] private Nation _nation;
    public Nation nation
    {
        get
        {
            return _nation;
        }	
    }


    [Header("Decorative:")]
    public string shipName;

    public void Initialise(ShipClassType shipClass, Nation shipNation)
    {
        _shipClass = shipClass;
        _nation = shipNation;

        shipName = DEFAULT_NAME;
    }    

    public string GetFullName()
    {
        if (nation.shipPrefix.Length > 0)
        {
            return nation.shipPrefix + " " + shipName;
        }
        else
        {
            return shipName;
        }
    }
}

}