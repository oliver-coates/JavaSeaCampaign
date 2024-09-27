using System;
using System.Collections;
using System.Collections.Generic;
using KahuInteractive.HassleFreeSaveLoad;
using Ships;
using UnityEngine;

public static class SessionMaster
{
    public static event Action OnLoaded;
    public static event Action OnPlayerCountChanged;
    public static event Action OnShipCountChanged;

    public static List<PlayerCharacter> playerCharacters;
    public static List<Ship> ships;


    public static void Initialise()
    {
        playerCharacters = new List<PlayerCharacter>();
        ships = new List<Ship>();
    
        ObjectRequestHandler.Initialise();
        LoadDataFromSaveFile();
    
        OnLoaded?.Invoke();
    }

    private static void LoadDataFromSaveFile()
    {
        List<SerializedObject> serializedObjects = SaveLoad.SerializedObjects;

        foreach (SerializedObject loadedObject in serializedObjects)
        {
            if (loadedObject is PlayerCharacter)
            {
                AddPlayerCharacter((PlayerCharacter) loadedObject);
            }
            else if (loadedObject is Ship)
            {
                AddShip((Ship) loadedObject);
            }
        } 
    }

    #region Player Characters
    public static void AddPlayerCharacter(PlayerCharacter playerCharacter)
    {
        playerCharacters.Add(playerCharacter);

        OnPlayerCountChanged?.Invoke();
    }

    public static void RemovePlayerCharacter(PlayerCharacter playerCharacter)
    {
        playerCharacters.Remove(playerCharacter);
        SaveLoad.UntrackSerializedObject(playerCharacter);

        OnPlayerCountChanged?.Invoke();
    }
    #endregion

    #region Ships

    public static void AddShip(Ship toAdd)
    {
        ships.Add(toAdd);

        OnShipCountChanged?.Invoke();
    }

    public static void RemoveShip(Ship toRemove)
    {
        ships.Remove(toRemove);
        SaveLoad.UntrackSerializedObject(toRemove);
        
        GameObject.Destroy(toRemove);

        OnShipCountChanged?.Invoke();
    }

    #endregion


}
