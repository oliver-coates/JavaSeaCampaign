using System.Collections;
using System.Collections.Generic;
using Effects;
using Ships;
using UnityEngine;

public class ShellInstance : BoardPiece
{
    #region Magic Numbers

    private const float SCALE_BASE = 0.75f;
    private const float SCALE_PER_HEIGHT = 0.1f;

    private const float COLLISION_MAX_HEIGHT = 5f;

    private const float ARM_TIME = 0.2f;

    #endregion

    [Header("Settings:")]
    [SerializeField] private AmmunitionType _type;
    [SerializeField] private float _distance;
    [SerializeField] private float _launchAngle;

    [Header("References:")]
    [SerializeField] private TrailRenderer _trail;

    [Header("State:")]
    [SerializeField] private float _height;
    [SerializeField] private float _flightTime;
    private Vector3 _positionLastFrame;

    protected override void Initialise()
    {
        _flightTime = 0f;

        // Ensure we are 'above' the map
        transform.position = new Vector3(transform.position.x, transform.position.y, -3f);
    }

    public void Fire(AmmunitionType type, float distance)
    {
        _type = type;
        _distance = distance;

        float initialVelocity = type.velocity;

        // Deterime launch angle to hit the distance
        _launchAngle = 0.5f * Mathf.Asin((9.81f * distance) / Mathf.Pow(initialVelocity, 2f));
        _launchAngle = _launchAngle * Mathf.Rad2Deg;

        _positionLastFrame = transform.position;
    }


    protected override void GameTick()
    {
         
    }


    protected override void UpdateTick()
    {
        
        
    }

    private void Update()
    {
        DetermineHeight();
        
        ScaleLineRendererByHeight();
    
        if (_height < COLLISION_MAX_HEIGHT &&
            _flightTime > ARM_TIME)
        {
            CheckForCollisions();
        }

        Move();

        _flightTime += Time.deltaTime;   
        _positionLastFrame = transform.position;
    }

    private void DetermineHeight()
    {
        // Convert launch angle to radians
        float a = _launchAngle * Mathf.Deg2Rad;

        // Assume Velocity stays constant 
        float v = _type.velocity;

        float gravity = 9.81f;

        // // Use formula to deterime height -> https://www.desmos.com/calculator/gjnco6mzjo
        // _height = (-4.9f * Mathf.Pow((_flightTime/(v * Mathf.Cos(a))), 2)) + Mathf.Tan(a);
    

        _height = (v * Mathf.Sin(a) * _flightTime) - (0.5f * gravity * Mathf.Pow(_flightTime, 2f));
    }

    private void ScaleLineRendererByHeight()
    {
        float scale = SCALE_BASE + (SCALE_PER_HEIGHT * _height);
        _trail.widthMultiplier = scale;
    }

    private void CheckForCollisions()
    {
        // Check first for collisions with enemy ships:
        RaycastHit2D hit = Physics2D.Linecast(transform.position, _positionLastFrame);
        if (hit.collider != null)
        {
            ShipSection hitSection = hit.collider.GetComponent<ShipSection>();
            if (hitSection != null)
            {
                // Debug.Log("Hit Ship!");
                EffectManager.SpawnEffect(_type.explosionEffect, transform.position);
                Destroy(gameObject);
            }
        }


        if (_height < 0)
        {
            // Hit the sea
            EffectManager.SpawnEffect(_type.splashEffect, transform.position);
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        transform.position += transform.up * Time.deltaTime * _type.velocity;
    }
}
