using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EngineScript : BoardPiece
{

    private ShipInstance _ship;

    [Header("State:")]
    // The velocity, in knots, of the ship
    [SerializeField] private float _velocity;

    // The speed that the engine is currently going.
    [Range(0,1)] [SerializeField] private float _engineSpeed;
    

    [Header("Settings:")]
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _engineAccelerationTime;
    private float _engineAcceleration
    {
        get
        {
            return 1f/_engineAccelerationTime;
        }
    }
    [SerializeField] private float _engineForce;
    [SerializeField] private float _turnSpeed;

    [SerializeField] private AnimationCurve _dragCurve;

    protected override void Initialise()
    {
        _ship = GetComponent<ShipInstance>();
        if (!_ship)
        {
            Debug.LogError($"No ship instance on ship engine");
        }
    }


    protected override void GameTick()
    {
        ApplyForce();
    }

    
    protected override void UpdateTick()
    {
        
    }


    private void ApplyForce()
    {
        float targetSpeed = _ship.targetSpeed;
        float rudder = _ship.rudder;

        // Accelerate the engine by engine acceleration amount:
        float accelerationTuner = 1f;
        if (_engineSpeed > targetSpeed)
        {
            // If we are decellerating, increase the speed at which this happens
            accelerationTuner = 2.5f;
        }
        _engineSpeed = Mathf.MoveTowards(_engineSpeed, targetSpeed, Time.deltaTime * _engineAcceleration * accelerationTuner);

        // Find how much velocity is to be gained this frame
        float velocityGain = _engineSpeed * _engineForce * (1f - _dragCurve.Evaluate(_velocity / _maxVelocity));

        // Add the engine speed onto veloicty
        _velocity += velocityGain * Time.deltaTime;

        // Move the ship forward by velocity
        // Note 1 knot is 0.5144 m/s
        transform.position += transform.forward * _velocity * 0.5144f * Time.deltaTime;

        // When the target speed is low, apply up to a 15% per frame velocity loss
        float decelerateFactor = (1 - _engineSpeed);
        _velocity -= _velocity * 0.15f * Time.deltaTime * decelerateFactor;

        // Rotate the ship by turn speed
        float turnAmount = GetTurnAmount(rudder);
        transform.Rotate(0, turnAmount * Time.deltaTime, 0);
    }

    private float GetTurnAmount(float rudder)
    {
        // How much the boat is turning at full speed
        float turnBase = rudder * _turnSpeed; 

        // Reduce the turn speed by the speed of the ship (0-1 range)
        float speedAmount = (_velocity / _maxVelocity);

        turnBase = turnBase * speedAmount;

        return turnBase;
    }


}
