using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Engine
{
    private Transform _targetTransform;
    private EngineScript _engineScript;

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

    /// <summary>
    /// Engine setup for simulator engines:
    /// </summary>
    /// <param name="copyFrom"></param>
    /// <param name="target"></param>
    public Engine(Engine copyFrom, Transform target)
    {
        _targetTransform = target;
        _engineScript = copyFrom._engineScript;

        _maxVelocity = copyFrom._maxVelocity;
        _engineAccelerationTime = copyFrom._engineAccelerationTime;
        _engineForce = copyFrom._engineForce;
        _turnSpeed = copyFrom._turnSpeed;
        _dragCurve = copyFrom._dragCurve;

    }

    public void Setup(Transform target, EngineScript engine)
    {
        _targetTransform = target;
        _engineScript = engine;
    }

    public void Sync(Engine master)
    {
        // Synchronises the real-time stats from the master engine:
        _velocity = master._velocity;
        _engineSpeed = master._engineSpeed;
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

    public void ApplyForce(float timeStep)
    {
        float targetSpeed = _engineScript.ship.targetSpeed;
        float rudder = _engineScript.ship.rudder;

        // Accelerate the engine by engine acceleration amount:
        float accelerationTuner = 1f;
        if (_engineSpeed > targetSpeed)
        {
            // If we are decellerating, increase the speed at which this happens
            accelerationTuner = 2f;
        }
        _engineSpeed = Mathf.MoveTowards(_engineSpeed, targetSpeed, timeStep * _engineAcceleration * accelerationTuner);

        // Find how much velocity is to be gained this frame
        float velocityGain = _engineSpeed * _engineForce * (1f - _dragCurve.Evaluate(_velocity / _maxVelocity));

        // Add the engine speed onto veloicty
        _velocity += velocityGain * timeStep;

        // Move the ship forward by velocity
        // Note 1 knot is 0.5144 m/s
        _targetTransform.position += _targetTransform.forward * _velocity * 0.5144f * timeStep;

        // When the target speed is low, apply up to a 15% per frame velocity loss
        float decelerateFactor = (1 - _engineSpeed);
        _velocity -= _velocity * 0.15f * timeStep * decelerateFactor;

        // Turn the ship by turn speed
        float turnAmount = GetTurnAmount(rudder);
        _targetTransform.Rotate(0, turnAmount * timeStep, 0);
    }
}
