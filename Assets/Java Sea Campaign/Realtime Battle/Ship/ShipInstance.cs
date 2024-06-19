using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInstance : MonoBehaviour
{


    [Header("Input:")]
    // The current state that this ships rudder is set to,
    // -1 being full port, 1 being full starboard
    [Range(-1,1)] public float rudder;

    // The target speed, that the ship's engine is accelerating towards
    // 0.0 = stop
    // 0.2 = slow ahead
    // 0.4 = half ahead
    // 0.6 = cruise ahead
    // 0.8 = full ahead
    // 1.0 = overtime ahead
    [Range(0,1)] public float targetSpeed;


    [Header("Components:")]
    [SerializeField] private EngineScript _engine;

    private void Awake()
    {
        GameMaster.onTurnEnd += OnTurnEnded;
    }

    private void Update()
    {
        if (GameMaster.turnUnderway)
        {
            _engine.TurnUpdate(rudder, targetSpeed, Time.deltaTime);
        }
    }

    private void OnShipStatChanged()
    {
        // Should be called whenever the target speed, rudder etc is changed.
        // SimulateTrajectory();
    }

    private void OnTurnEnded()
    {
        // SimulateTrajectory();
    }

}
