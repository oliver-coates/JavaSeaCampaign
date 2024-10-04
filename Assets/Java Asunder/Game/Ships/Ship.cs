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
    // Called when any value changes on this here ship:
    public event Action OnChange;

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

    [SerializeField] private bool _isPlayerShip;
    public bool isPlayerShip
    {
        get
        {
            return _isPlayerShip;
        }	
    }

    [SerializeField] private bool _isIncludedInBattle;
    public bool isIncludedInBattle
    {
        get
        {
            return _isIncludedInBattle;
        }	
    }


    [Header("Decorative:")]
    [SerializeField] private string _shipName;
    public string shipName
    {
        get
        {
            return _shipName;
        }	
    }



    public void Initialise(ShipClassType shipClass, Nation shipNation)
    {
        _shipClass = shipClass;
        _nation = shipNation;

        _shipName = DEFAULT_NAME;
        _isIncludedInBattle = false;

        OnChange?.Invoke();
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

    public void Rename(string newName)
    {
        _shipName = newName;

        OnChange?.Invoke();
    }

    public void ChangeNation(Nation nation)
    {
        _nation = nation;

        OnChange?.Invoke();
    }

    public void SetIncludedInBattle(bool isIncludedInBattle)
    {
        _isIncludedInBattle = isIncludedInBattle;
        OnChange?.Invoke();
    }

    public void SetIsPlayerShip(bool isPlayerShip)
    {
        _isPlayerShip = isPlayerShip;

        if (isPlayerShip == false)
        {
            SessionMaster.PlayerShip = null;
        }
        else
        {
            SessionMaster.PlayerShip = this;
        }

        OnChange?.Invoke();
    }
}



}