using System.Collections;
using System.Collections.Generic;
using Ships;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class EngineScript : BoardPiece
{

    #region Magic Tuners

    private const float ENGINE_SPEED_STARTING = 0.6f;

    private const float SHIP_DRAG = 15f;
    private const float SHIP_DRAG_ANGULAR = 1.15f;
    private const float SPEED_TUNER = 50000;
    private const float TURN_SPEED_TUNER = 10000;
    private const float ENGINE_CHANGE_SPEED_TUNER = 0.0005f;
    private const float ENGINE_SPEED_CHANGE_DOWN_MULTIPLIER = 1.5f;
    #endregion

    [SerializeField] private ComponentSlot _engineSlot;
    [SerializeField] private Rigidbody2D _rigidBody;
    private ShipInstance _ship;
    private EngineType _engineType;

    [Header("State:")]
    // The speed that the engine is currently going.
    [Range(0,1)] [SerializeField] private float _engineSpeed;
    public float engineSpeed
    {
        get
        {
            return _engineSpeed;
        }
    }

    protected override void Initialise() 
    {
        _ship = GetComponentInParent<ShipInstance>();

        if (_engineSlot.component is not EngineType)
        {
            Debug.LogError($"Provided component in engine slot is not Engine Type");
            return;
        }

        _engineType = (EngineType) _engineSlot.component;

        _rigidBody.drag = SHIP_DRAG;
        _rigidBody.angularDrag = SHIP_DRAG_ANGULAR;
    
        _engineSpeed = ENGINE_SPEED_STARTING;
    }

    protected override void UpdateTick() { }


    protected override void GameTick()
    {
        // Spool up/down the engine
        float engineChangeSpeed = _engineType.agility * ENGINE_CHANGE_SPEED_TUNER;
        // The engine spools down faster than spooling up
        if (_engineSpeed > _ship.targetSpeed)
        {
            engineChangeSpeed *= ENGINE_SPEED_CHANGE_DOWN_MULTIPLIER;
        }
        _engineSpeed = Mathf.MoveTowards(_engineSpeed, _ship.targetSpeed, engineChangeSpeed * Time.deltaTime);

        // Add force:
        Vector3 force = _ship.transform.up * _engineType.strength * _engineSpeed * Time.deltaTime * SPEED_TUNER;
        _rigidBody.AddForce(force, ForceMode2D.Force);
    
        // Rotate:
        float rotateAmount = _ship.rudder * TURN_SPEED_TUNER * Time.deltaTime;

        rotateAmount *= _rigidBody.velocity.magnitude;
        _rigidBody.AddTorque(rotateAmount, ForceMode2D.Force);
    }

}
