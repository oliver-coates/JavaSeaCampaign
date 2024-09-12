using System;
using System.Collections;
using System.Collections.Generic;
using KahuInteractive.HassleFreeSaveLoad;
using UnityEngine;

public static class SessionMaster
{
    public static event Action OnLoaded;

    public static List<PlayerCharacter> playerCharacters;

    public static void Initialise()
    {
        playerCharacters = new List<PlayerCharacter>();
    
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
                playerCharacters.Add((PlayerCharacter) loadedObject);
            }
        } 
    }
}
