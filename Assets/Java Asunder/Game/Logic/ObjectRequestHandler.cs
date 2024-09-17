using System;
using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using KahuInteractive.UIHelpers;

public static class ObjectRequestHandler
{
    
    private static BasicSelection.Category<ShipClassType>[] _shipTypeCategories;

    public static void Initialise()
    {
        AssembleShipTypesIntoCategories();
    }

    private static void AssembleShipTypesIntoCategories()
    {
        _shipTypeCategories = new BasicSelection.Category<ShipClassType>[2];
        
        int i = 0;

        // Hard coding all types lol - this sucks:        

        #region Destroyers
        ShipClassType[] destroyerTypes = Resources.LoadAll<ShipClassType>("Ship Classes/Destroyers");
        BasicSelection.Option<ShipClassType>[] destroyerOptions = new BasicSelection.Option<ShipClassType>[destroyerTypes.Length]; 
        i = 0;
        foreach (ShipClassType destroyerType in destroyerTypes)
        {
            destroyerOptions[i] = new BasicSelection.Option<ShipClassType>(destroyerType.name, destroyerType);
            
            i++;
        }

        BasicSelection.Category<ShipClassType> destroyerCategory = new BasicSelection.Category<ShipClassType>("Destroyers", destroyerOptions);
        _shipTypeCategories[0] = destroyerCategory;
        #endregion

        #region Freighter
        ShipClassType[] freighterTypes = Resources.LoadAll<ShipClassType>("Ship Classes/Freighters");
        BasicSelection.Option<ShipClassType>[] freighterOptions = new BasicSelection.Option<ShipClassType>[freighterTypes.Length]; 
        i = 0;
        foreach (ShipClassType freighterType in freighterTypes)
        {
            freighterOptions[i] = new BasicSelection.Option<ShipClassType>(freighterType.name, freighterType);
            
            i++;
        }

        BasicSelection.Category<ShipClassType> freighterCategory = new BasicSelection.Category<ShipClassType>("Freighters", freighterOptions);
        _shipTypeCategories[1] = freighterCategory;
        #endregion
    }
    

    public static void RequestShipClass(Action<ShipClassType> onFulfilled)
    {
        BasicSelection.RequestSelection(_shipTypeCategories, onFulfilled);
    }
}
