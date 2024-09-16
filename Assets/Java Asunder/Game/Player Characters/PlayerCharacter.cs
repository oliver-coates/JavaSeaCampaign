using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KahuInteractive.HassleFreeSaveLoad;
using System;


[Serializable]
public class PlayerCharacter : SerializedObject
{
    [Header("Character:")]
    [SerializeField] public string characterName = "New Player Character";

    [SerializeField] public string imagePath = "";
    public Sprite image = null; // Not serialized
    
    [SerializeField] public bool isDisabled = false;

    [Header("Task:")]
    [SerializeField] public string currentTask = "No Task";
    [SerializeField] public float timeToCompleteTask = 0;
    [SerializeField] public float taskTimer = 0;



}
