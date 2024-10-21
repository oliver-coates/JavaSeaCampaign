using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Ships
{


public class ShipInstance : BoardPiece
{
    public static event Action<ShipInstance> OnShipInstanceCreated;
    public static event Action<ShipInstance> OnShipInstanceDestroyed;
    public static event Action<ShipInstance> OnPlayerShipCreated;


    public event Action<ShipInstance> OnTargetSet;


    [Header("Ship Data:")]
    public Ship shipData;

    [Header("Decoration:")]
    public float UIDisplayOffset = -50f;
    public LineRenderer shipDirectionLine; // A direction line only visible to the game master
    public LineRenderer shipTargetLine; // A line visible only to the game master, shows what the ship is targetting with its guns.

    [Header("Sections:")]
    [HideInInspector] public ShipAIController AI; // Will only exist on non-player ships
    public ComponentSlot armourSlot;
    [HideInInspector] public ShipSection[] sections;
    [HideInInspector] public ComponentSlot[] componentSlots;

    [Header("Easy component access")]
    public Rigidbody2D rb;
    public ComponentSlot bridge;


    [Header("Core Components: (Filled out at runtime)")]
    public EngineScript engine;
    public FireControlInstance fireControl;


    [Header("Input:")]
    // The current state that this ships rudder is set to,
    // -1 being full port, 1 being full starboard
    [SerializeField] [Range(-1,1)] private float _rudder;
    public float rudder
    {
        get
        {
            return _rudder;
        }

        set
        {
            _rudder = value;
        }
    }

    // The target speed, that the ship's engine is accelerating towards
    // 0.0 = stop
    // 0.25 = slow ahead
    // 0.5 = half ahead
    // 0.75 = cruise ahead
    // 1.0 = full ahead
    [SerializeField] [Range(0,1)] private float _targetSpeed;
    public float targetSpeed
    {
        get
        {
            return _targetSpeed;
        }

        set
        {
            _targetSpeed = value;
        }
    }

    [SerializeField] private ShipInstance _target;
    public ShipInstance target
    {
        get
        {
            return _target;
        }	
    }



    protected override void Initialise()
    {
        OnShipInstanceCreated?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnShipInstanceDestroyed?.Invoke(this);
    }


    public void Setup(Ship ship)
    {
        shipData = ship;
        ship.instance = this;
        name = ship.GetFullName();

        sections = GetComponentsInChildren<ShipSection>();

        // Intiialsie all sections & Gather all component slots
        List<ComponentSlot> allSlots = new List<ComponentSlot>();
        allSlots.Add(armourSlot);

        foreach (ShipSection section in sections)
        {
            section.Initialise(this);
            allSlots.AddRange(section.slots);
        }

        componentSlots = allSlots.ToArray();

        if (ship != SessionMaster.PlayerShip)
        {
            // Since this is not a player ship, it needs to be controlled by an AI 
            AI = gameObject.AddComponent<ShipAIController>();
        }
        else
        {
            // This is a player ship
            OnPlayerShipCreated?.Invoke(this);
            SessionMaster.MoveAllPlayerCharactersToBridge(this);
        }
    }


    protected override void GameTick()
    {

    }


    protected override void UpdateTick()
    {
        UpdateDirectionLine();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 tagPos = transform.position + new Vector3(0, UIDisplayOffset, 0);    
        Gizmos.DrawLine(transform.position, tagPos);
        
        Vector3 boxSizeApprox = new Vector3(10, 5, 0.1f);
        Gizmos.DrawCube(tagPos, boxSizeApprox);
        
    }

    public void SetTarget(ShipInstance shipInstance)
    {
        _target = shipInstance;
        OnTargetSet?.Invoke(shipInstance);
    }

    private void UpdateDirectionLine()
    {
        if (shipData.isSelectedByGameMaster && _target != null)
        {
            shipTargetLine.enabled = true;

            Vector3[] positions = new Vector3[2];
            positions[0] = transform.position;
            positions[1] = _target.transform.position;

            shipTargetLine.SetPositions(positions);
        }
        else
        {
            shipTargetLine.enabled = false;
        }
    }
}

}