using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInstance : BoardPiece
{
    

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


    [Header("Components:")]
    [SerializeField] private EngineScript _engine;
    // [SerializeField] private LineRenderer _lineRenderer;

    protected override void Initialise()
    {
    }


    protected override void GameTick()
    {

    }


    protected override void UpdateTick()
    {
    }

    
}
