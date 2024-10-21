using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KahuInteractive.HassleFreeSaveLoad;
using System;
using Ships;


[Serializable]
public class PlayerCharacter : SerializedObject
{
    public const float MOVEMENT_TIME_BASE = 3f;
    public const float MOVEMENT_TIME_PER_METER_TRAVELLED = 1f;

    public event Action OnStateChanged;

    [Header("Character:")]
    [SerializeField] public string characterName = "New Player Character";

    [SerializeField] public string imagePath = "";
    public Sprite image = null; // Not serialized
    
    [SerializeField] public bool isDisabled = false;

    
    [Header("Decorative")]
    public string currentTaskString = "No Task";
    
    [Header("Task:")]
    
    [SerializeField] private ComponentSlot _location;
    [SerializeField] private ComponentSlot _moveDestination; // For movement from one component slot to another
    [SerializeField] private int _roll; // For applying a buff to a component instance
    [SerializeField] private TaskType _taskType;
    private enum TaskType
    {
        MoveTo,
        ApplyBuff
    }
    [Header("Timer:")]
    private bool _hasTask;
    [SerializeField] public float timeToCompleteTask = 0;
    [SerializeField] public float taskTimer = 0;

    protected override void Initialise()
    {
        _location = null;
        _moveDestination = null;
    }

    public void Tick(float deltaTime)
    {
        if (_hasTask)
        {
            taskTimer += deltaTime;

            if (taskTimer > timeToCompleteTask)
            {
                if (_taskType == TaskType.MoveTo)
                {
                    FinishMoveTo();
                }
            }
        }
    }
    

    public void TeleportTo(ComponentSlot destination)
    {
        _moveDestination = destination;
        FinishMoveTo();
    }

    public void StartMoveTo(ComponentSlot destination)
    {
        _moveDestination = destination;

        float distance = Vector3.Distance(_location.transform.position, _moveDestination.transform.position);

        taskTimer = 0f;
        timeToCompleteTask = MOVEMENT_TIME_BASE + (distance * MOVEMENT_TIME_PER_METER_TRAVELLED);
        _hasTask = true;

        currentTaskString = $"Moving from the {_location.slotName} to the {destination.slotName}";
        
        OnStateChanged?.Invoke();
    }

    public void FinishMoveTo()
    {
        _location = _moveDestination;
        _moveDestination = null;

        taskTimer = 0f;
        _hasTask = false;

        currentTaskString = $"At the {_location.slotName}";

        OnStateChanged?.Invoke();
    }



}
