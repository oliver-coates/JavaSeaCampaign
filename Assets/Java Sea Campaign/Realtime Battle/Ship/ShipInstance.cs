using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInstance : MonoBehaviour
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
            OnShipStatChanged();
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
            OnShipStatChanged();
        }
    }


    [Header("Components:")]
    [SerializeField] private EngineScript _engine;
    [SerializeField] private LineRenderer _lineRenderer;


    private void Awake()
    {
        GameMaster.onTurnEnd += OnTurnEnded;
    }

    private void Update()
    {
        if (GameMaster.turnUnderway)
        {
            _engine.TurnUpdate(Time.deltaTime);
        }
    }

    private void OnShipStatChanged()
    {
        // Should be called whenever the target speed, rudder etc is changed.
        RedrawTrajectory();
    }

    private void OnTurnEnded()
    {
        RedrawTrajectory();
    }

    private void RedrawTrajectory()
    {
        _lineRenderer.positionCount = 100;
        _lineRenderer.SetPositions(_engine.SimulateMovement());

        float baseWidth = 0.05f;
        float steerLerp = Mathf.Abs(rudder);

        _lineRenderer.startWidth = baseWidth;
        _lineRenderer.endWidth = Mathf.Lerp(baseWidth, 6f, steerLerp);

        Color endColor = Color.Lerp(Color.black, Color.clear, steerLerp);

        _lineRenderer.endColor = endColor;
    }

}
