using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineScript : MonoBehaviour
{

    private float _inputRudder;
    private float _inputTargetSpeed;

    [Header("State:")]
    // The velocity, in knots, of the ship
    [SerializeField] private float _velocity;

    // The speed that the engine is currently going.
    [Range(0,1)] [SerializeField] private float _engineSpeed;
    
    // The degrees that the ship turns this second    
    private float _turnAmount
    {
        get
        {
            // How much the boat is turning at full speed
            float turnBase = _inputRudder * _turnSpeed; 

            // Reduce the turn speed by the speed of the ship (0-1 range)
            float speedAmount = (_velocity / maxVelocity);

            turnBase = turnBase * speedAmount;

            return turnBase;
        }
    }


    [Header("Settings:")]
    public float maxVelocity;
    public float engineAccelerationTime;
    private float _engineAcceleration
    {
        get
        {
            return 1f/engineAccelerationTime;
        }
    }
    public float _engineForce;
    public float _turnSpeed;

    [SerializeField] private AnimationCurve _dragCurve;

    [Header("References:")]
    [SerializeField] private LineRenderer _lineRenderer;


    public void TurnUpdate(float rudder, float targetSpeed, float timeStep)
    {
        _inputRudder = rudder;
        _inputTargetSpeed = targetSpeed;

        // Accelerate the engine by engine acceleration amount:
        float accelerationTuner = 1f;
        if (_engineSpeed > _inputTargetSpeed)
        {
            // If we are decellerating, increase the speed at which this happens
            accelerationTuner = 2f;
        }
        _engineSpeed = Mathf.MoveTowards(_engineSpeed, _inputTargetSpeed, timeStep * _engineAcceleration * accelerationTuner);

        // Find how much velocity is to be gained this frame
        float velocityGain = _engineSpeed * _engineForce * (1f - _dragCurve.Evaluate(_velocity / maxVelocity));

        // Add the engine speed onto veloicty
        _velocity += velocityGain * timeStep;

        // Move the ship forward by velocity
        // Note 1 knot is 0.5144 m/s
        transform.position += transform.forward * _velocity * 0.5144f * timeStep;

        // When the target speed is low, apply up to a 15% per frame velocity loss
        float decelerateFactor = (1 - _engineSpeed);
        _velocity -= _velocity * 0.15f * timeStep * decelerateFactor;

        // Turn the ship by turn speed
        transform.Rotate(0,  _turnAmount * timeStep, 0);
    }
}
