using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

public class ShipGunScript : BoardPiece, IShipComponentInstance
{
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
            dir = aimLocation -  transform.position;
        }
        else
        {
            // If no target, aim straight ahead
            dir = transform.position + (transform.up * 10f);
        }

        TurnTurretTowards(dir);
    }

    private void TurnTurretTowards(Vector3 direction)
    {
        Quaternion targetDir = Quaternion.LookRotation(direction);

        _turret.rotation = Quaternion.Slerp(_turret.rotation, targetDir, Time.deltaTime * _gunType.turnSpeed);
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