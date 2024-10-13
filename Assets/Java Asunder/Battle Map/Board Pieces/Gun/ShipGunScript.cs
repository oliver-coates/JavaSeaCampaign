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
        Debug.DrawLine(transform.position, transform.position + direction, Color.yellow);


        Vector3 directionRelativeToTurret = transform.worldToLocalMatrix * direction;
        Debug.DrawLine(transform.position, transform.position + directionRelativeToTurret, Color.red);


        Quaternion targetRotation = Quaternion.LookRotation(directionRelativeToTurret, Vector3.up);

        float turnDelta = Time.deltaTime * _gunType.turnSpeed;
        turnDelta = 1;

        _turret.rotation = Quaternion.Slerp(_turret.rotation, targetRotation, turnDelta);
    }

    private void ReturnToRestPosition()
    {
        _turret.localRotation = Quaternion.Slerp(_turret.localRotation, Quaternion.identity, Time.deltaTime * _gunType.turnSpeed);
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