using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

public class ShipAIController : BoardPiece
{
    ShipInstance _ship;

    [Header("State:")]
    [SerializeField] private Vector3 _targetDestination;
    [SerializeField] private ShipInstance _targetShip;

    private void Awake()
    {
        _ship = GetComponent<ShipInstance>();

        _targetDestination = new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f));
    
    }

    protected override void GameTick()
    {
    }

    protected override void Initialise()
    {
    }

    protected override void UpdateTick()
    {
        UpdateDirectionLineRenderer();
    }

    private void UpdateDirectionLineRenderer()
    {
        if (_ship.shipData.isSelectedByGameMaster)
        {
            _ship.shipDirectionLine.enabled = true;

            Vector3[] lineRendererPositons = new Vector3[2];
            lineRendererPositons[0] = transform.position;
            lineRendererPositons[1] = _targetDestination;

            _ship.shipDirectionLine.SetPositions(lineRendererPositons);
        }
        else
        {
            _ship.shipDirectionLine.enabled = false;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        _targetDestination = destination;
    }

    public void SetTarget(ShipInstance ship)
    {
        _targetShip = ship;
    }
}
