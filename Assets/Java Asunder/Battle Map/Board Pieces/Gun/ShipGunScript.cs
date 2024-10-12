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
    public Transform target;


    [Header("Settings:")]
    [SerializeField] private ShipGunType _gunType;
    // [SerializeField] private AmmunitionType _ammoType;

    
    public void Setup(ShipInstance ship, ComponentSlot componentSlot)
    {
        _ship = ship;
        _componentSlot = componentSlot;

        if (_componentSlot.component is not ShipGunType)
        {
            Debug.LogError($"Provided component in engine slot is not Gun Type");
            return;
        }

        _gunType = (ShipGunType) _componentSlot.component;
    }

    protected override void Initialise() {  }

    protected override void GameTick()
    {
        TurnTurret();
    }

    protected override void UpdateTick()
    {
    }

    
    #region Turret Rotation
    private void TurnTurret()
    {
        Vector3 dir;

        if (target != null)
        {
            dir = target.position -  transform.position;
        }
        else
        {
            // If no target, aim straight ahead
            dir = target.position + (transform.up * 10f);
        }

        TurnTurretTowards(dir);
    }

    private void TurnTurretTowards(Vector3 direction)
    {

    }
    #endregion

}

}