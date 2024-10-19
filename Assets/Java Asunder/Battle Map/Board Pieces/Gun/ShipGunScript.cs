using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

public class ShipGunScript : BoardPiece, IShipComponentInstance
{
    #region Magic numbers

    private const float TURRET_TURN_DEAD_ZONE = 0.05f;
    private const float TURRET_FIRE_ANGLE_ALLOWANCE = 0.9f; // How close the forward dot product must alight to the target for shots to be allowed to be fired
    private const float TURRET_RELOAD_RANDOMISATION = 0.1f;

    private const float TARGET_BIAS_LENGTH = 150f;

    #endregion

    private ComponentSlot _componentSlot;
    private ShipInstance _ship;

    [Header("State:")]
    public ShipInstance _target;
    [SerializeField] private bool _pointingTowardsTarget;
    [SerializeField] private float _distanceToTarget;
    private float _loadTimer;

    public ComponentEffectiveness loadingEffectiveness;

    [SerializeField] private Vector2 _shotBias;

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
        loadingEffectiveness = new ComponentEffectiveness();

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

        GunUpdate();
    
        loadingEffectiveness.Tick();
    }

    protected override void UpdateTick()
    {
    }

    
    private void SetTarget(ShipInstance target)
    {
        _target = target;
        GenerateNewShotBias();
    }



    #region Turret Rotation
    private void TurnTurret()
    {
        Vector3 dir;

        if (_target != null)
        {
            Vector3 aimLocation = GetAimLocation();
            dir = (aimLocation -  transform.position).normalized;

            _distanceToTarget = Vector3.Distance(transform.position, aimLocation);

            Debug.DrawLine(transform.position, aimLocation, Color.red);

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

        _pointingTowardsTarget = dotForward > TURRET_FIRE_ANGLE_ALLOWANCE;
    }


    #endregion

    #region Gunnery

    /// <summary>
    /// Gets the location, as a vector3, that this turret thinks is the enemy ship position
    /// </summary>
    private Vector3 GetAimLocation()
    {
        // Deterime the actual position of where the target is:
        float timeToReachTarget = _distanceToTarget / _gunType.ammo.velocity;
        Vector3 velocityOffset = _target.rb.velocity * timeToReachTarget;
        Vector3 velocityAdjustedTargetPosition = _target.transform.position + velocityOffset;

        // Add in the shot bias
        Vector2 shotBiasAmount = _shotBias * TARGET_BIAS_LENGTH;
        if (_ship.fireControl != null)
        {
            // if we have a fire control system, reduce the shot bias
            float accuracyMultiplier = 1f- (_ship.fireControl.confidence / 100f);
            shotBiasAmount = shotBiasAmount * accuracyMultiplier;
        }

        Vector3 finalLocation = velocityAdjustedTargetPosition;
        finalLocation.x += shotBiasAmount.x;
        finalLocation.y += shotBiasAmount.y;

        return finalLocation;
    }

    private void GunUpdate()
    {
        // The crew consistently loads the gun
        _loadTimer += (Time.deltaTime * loadingEffectiveness.value);

        if (_target is not null)
        {
            if (_loadTimer > _gunType.reloadTime && _pointingTowardsTarget)
            {
                Shoot();
                float reloadTimeRange = TURRET_RELOAD_RANDOMISATION * _gunType.reloadTime;
                _loadTimer = Random.Range(-reloadTimeRange, reloadTimeRange);
            }
        }


    }

    private void Shoot()
    {
        GameObject shellObj = Instantiate(_gunType.ammo._prefab);

        shellObj.transform.position = _shootPoint.position;
        shellObj.transform.position = new Vector3(shellObj.transform.position.x,
                                                 shellObj.transform.position.y,
                                                  -3f);
        shellObj.transform.rotation = _shootPoint.rotation;

        // Add random rotation
        float inaccuracy = Random.Range(-(_gunType.inaccuracy/2f), _gunType.inaccuracy/2f);
        shellObj.transform.Rotate(0, 0, inaccuracy);

        ShellInstance shell = shellObj.GetComponent<ShellInstance>();
        shell.Fire(_gunType.ammo, _distanceToTarget);
    }

    private void GenerateNewShotBias()
    {
        _shotBias = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    #endregion
}

}