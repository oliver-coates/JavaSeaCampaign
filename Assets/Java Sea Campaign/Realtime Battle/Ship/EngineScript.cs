using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EngineScript : MonoBehaviour
{

    public ShipInstance ship;

    [Header("Engine:")]
    [SerializeField] private Engine _engine;
    [HideInInspector] private Engine _simulatorEngine;
    

    [Header("References:")]
    private Transform _simulatorTransform;


    private void Awake()
    {
        _simulatorTransform = new GameObject("Engine Simulator").transform;
        _simulatorTransform.SetParent(transform);
    
        // _engine states should be set in inspector for now (rework this to use ship sheet later)
        _engine.Setup(transform, this);
        _simulatorEngine = new Engine(_engine, _simulatorTransform);
    }

    public void TurnUpdate(float timeStep)
    {
        _engine.ApplyForce(timeStep);
    }

    public Vector3[] SimulateMovement()
    {
        int steps = 100;
        float timeStep = 5f/steps;

        Vector3[] postionData = new Vector3[100];

        // Ensure we are starting at the ships position and rotation
        _simulatorTransform.position = transform.position;
        _simulatorTransform.rotation = transform.rotation;

        // Ensure that the simulator engines stats match the real engines stats:
        _simulatorEngine.Sync(_engine);

        for (int i = 0; i < steps; i++)
        {
            // Record where we are
            postionData[i] = _simulatorTransform.position;
            postionData[i].y = 0.1f;
        
            // Call a turn update to move us forward
            _simulatorEngine.ApplyForce(timeStep);
        }
    
        return postionData;
    }
}
