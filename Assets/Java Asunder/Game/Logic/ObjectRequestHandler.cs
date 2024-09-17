using System;
using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using KahuInteractive.UIHelpers;

public static class ObjectRequestHandler
{
    
    private static BasicSelectionManager.SelectionCategory<ShipClassType>[] _shipTypeCategories;

    public static void Initialise()
    {
        AssembleShipTypesIntoCategories();
    }

    private static void AssembleShipTypesIntoCategories()
    {
        _shipTypeCategories = new BasicSelectionManager.SelectionCategory<ShipClassType>[2];
        
        int i = 0;

        // Hard coding all types lol - this sucks:        

        #region Destroyers
        ShipClassType[] destroyerTypes = Resources.LoadAll<ShipClassType>("Ship Classes/Destroyers");
        BasicSelectionManager.SelectionOption<ShipClassType>[] destroyerOptions = new BasicSelectionManager.SelectionOption<ShipClassType>[destroyerTypes.Length]; 
        i = 0;
        foreach (ShipClassType destroyerType in destroyerTypes)
        {
            destroyerOptions[i] = new BasicSelectionManager.SelectionOption<ShipClassType>(destroyerType.name, destroyerType);
            
            i++;
        }

        BasicSelectionManager.SelectionCategory<ShipClassType> destroyerCategory = new BasicSelectionManager.SelectionCategory<ShipClassType>("Destroyers", destroyerOptions);
        _shipTypeCategories[0] = destroyerCategory;
        #endregion

        #region Freighter
        ShipClassType[] freighterTypes = Resources.LoadAll<ShipClassType>("Ship Classes/Freighters");
        BasicSelectionManager.SelectionOption<ShipClassType>[] freighterOptions = new BasicSelectionManager.SelectionOption<ShipClassType>[freighterTypes.Length]; 
        i = 0;
        foreach (ShipClassType freighterType in freighterTypes)
        {
            freighterOptions[i] = new BasicSelectionManager.SelectionOption<ShipClassType>(freighterType.name, freighterType);
            
            i++;
        }

        BasicSelectionManager.SelectionCategory<ShipClassType> freighterCategory = new BasicSelectionManager.SelectionCategory<ShipClassType>("Freighters", freighterOptions);
        _shipTypeCategories[1] = freighterCategory;
        #endregion
    }
    

    public static void RequestShipClass(Action<ShipClassType> onFulfilled)
    {
        BasicSelectionManager.RequestSelection(_shipTypeCategories, onFulfilled);
    }
}
