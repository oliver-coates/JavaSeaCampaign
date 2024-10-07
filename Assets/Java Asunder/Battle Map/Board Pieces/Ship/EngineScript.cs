using System.Collections;
using System.Collections.Generic;
using Ships;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class EngineScript : BoardPiece
{

    #region Magic Tuners

    private const float SHIP_DRAG = 0.15f;
    private const float SHIP_DRAG_ANGULAR = 0.1f;
    private const float GLOBAL_SPEED_TUNER = 4500;
    #endregion

    [SerializeField] private ComponentSlot _engineSlot;
    [SerializeField] private Rigidbody2D _rigidBody;
    private ShipInstance _ship;
    private EngineType _engineType;

    [Header("State:")]
    // The speed that the engine is currently going.
    [Range(0,1)] [SerializeField] private float _engineSpeed;

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
    }

    protected override void GameTick()
    {
        Vector3 force = _ship.transform.up * _engineType.strength * _engineSpeed * Time.deltaTime * GLOBAL_SPEED_TUNER;

        _rigidBody.AddForce(force, ForceMode2D.Force);
    }


    protected override void UpdateTick()
    {
    }
}
