using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

[System.Serializable]
public class EngineScript : BoardPiece, IShipComponentInstance
{

    #region Magic Tuners

    public const float ENGINE_SPEED_STARTING = 0.6f;

    private const float SHIP_DRAG = 15f;
    private const float SHIP_DRAG_ANGULAR = 1.15f;
    private const float SPEED_TUNER = 20000;
    private const float TURN_SPEED_TUNER = 25000;
    private const float ENGINE_CHANGE_SPEED_TUNER = 0.0005f;
    private const float ENGINE_SPEED_CHANGE_DOWN_MULTIPLIER = 1.5f;
    #endregion

    [SerializeField] private ComponentSlot _engineSlot;
    private SectionState _sectionState;
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

    public float speed
    {
        get
        {
            // returns the speed of this vessel, in knots
            return _rigidBody.velocity.magnitude * 1.943844f;
        }
    }

    public ComponentEffectiveness speedEffectivness;

    protected override void Initialise() { }

    public void Setup(ShipInstance ship, ComponentSlot componentSlot)
    {
        _ship = ship;
        _engineSlot = componentSlot;
        _sectionState = componentSlot.shipSection.state;
        _ship.engine = this;
        speedEffectivness = new ComponentEffectiveness("Engines", "Overcharging the engines");

        _engineType = (EngineType) _engineSlot.component;

        // Set up rigid body:
        _rigidBody = ship.rb;

        _rigidBody.drag = SHIP_DRAG;
        _rigidBody.angularDrag = SHIP_DRAG_ANGULAR;
    
        _engineSpeed = ENGINE_SPEED_STARTING;
    }

    protected override void UpdateTick() { }


    protected override void GameTick()
    {
        speedEffectivness.Tick();

        // Spool up/down the engine
        float engineChangeSpeed = _engineType.agility * ENGINE_CHANGE_SPEED_TUNER;
        // The engine spools down faster than spooling up
        if (_engineSpeed > _ship.targetSpeed)
        {
            engineChangeSpeed *= ENGINE_SPEED_CHANGE_DOWN_MULTIPLIER;
        }
        _engineSpeed = Mathf.MoveTowards(_engineSpeed, _ship.targetSpeed, engineChangeSpeed * Time.deltaTime);

        // Add force:
        float speedMultiplier = _engineType.strength * SPEED_TUNER * speedEffectivness.value * _sectionState.effectivenessMultiplier;
        Vector3 force = _ship.transform.up *  _engineSpeed * Time.deltaTime * speedMultiplier;
        _rigidBody.AddForce(force, ForceMode2D.Force);
    
        // Rotate:
        float rotationMultiplier = TURN_SPEED_TUNER; // TODO: Get rotation multilier from bridge
        float rotateAmount = _ship.rudder * rotationMultiplier * Time.deltaTime;

        rotateAmount *= _rigidBody.velocity.magnitude;
        _rigidBody.AddTorque(rotateAmount, ForceMode2D.Force);
    }

    public ComponentEffectiveness[] GetComponentEffectivenesses()
    {
        ComponentEffectiveness[] output = new ComponentEffectiveness[1];

        output[0] = speedEffectivness;

        return output;
    }
}
