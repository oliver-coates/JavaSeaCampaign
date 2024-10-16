using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

public class ShipGunScript : BoardPiece, IShipComponentInstance
{
    #region Magic numbers

    private const float TURRET_TURN_DEAD_ZONE = 0.05f;

    #endregion

    private ComponentSlot _componentSlot;
    private ShipInstance _ship;

    [Header("State:")]
    public ShipInstance _target;

    [Header("References:")]
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _shootPoint;

    [Header("Settings:")]
    [SerializeField] private ShipGunType _gunType;
    // [SerializeField] private AmmunitionType _ammoType;

    
    #region Initialisation & Destruction
    public void Setup(ShipInstance ship, ComponentSlot componentSlot)
    {
        _ship = ship;
        _componentSlot = componentSlot;

        if (_componentSlot.component is not ShipGunType)
        {
            Debug.LogError($"Provided component in gun slot is not Gun Type");
            return;
        }

        _gunType = (ShipGunType) _componentSlot.component;
    
        _ship.OnTargetSet += SetTarget;
    }

    private void OnDestroy()
    {
        _ship.OnTargetSet -= SetTarget;
    }
    #endregion

    protected override void Initialise() {  }

    protected override void GameTick()
    {
        TurnTurret();
    }

    protected override void UpdateTick()
    {
    }

    
    private void SetTarget(ShipInstance target)
    {
        _target = target;
    }



    #region Turret Rotation
    private void TurnTurret()
    {
        Vector3 dir;

        if (_target != null)
        {
            Vector3 aimLocation = GetAimLocation();
            dir = (aimLocation -  transform.position).normalized;
            TurnTurretTowards(dir);
        }
        else
        {
            // If no target, aim straight ahead            
            TurnTurretTowards(Vector3.up);
        }
    }

    private void TurnTurretTowards(Vector3 direction)
    {
        float dotRight = Vector3.Dot(direction, _turret.right);
        float dotForward = Vector3.Dot(direction, _turret.up);

        int turnDirection = 0;

        if (dotRight > TURRET_TURN_DEAD_ZONE)
        {
            turnDirection = -1;
        }
        else if (dotRight < (-TURRET_TURN_DEAD_ZONE))
        {
            turnDirection = 1;
        }
        if (dotForward < (-1 + TURRET_TURN_DEAD_ZONE))
        {
            turnDirection = 1;
        }

        _turret.Rotate(0, 0, turnDirection * _gunType.turnSpeed * Time.deltaTime);
    }


    #endregion

    #region Gunnery

    /// <summary>
    /// Gets the location, as a vector3, that this turret thinks is the enemy ship position
    /// </summary>
    private Vector3 GetAimLocation()
    {
        // TODO: Add in inaccuracy, etc here
        
        return _target.transform.position;
    }

    #endregion
}

}