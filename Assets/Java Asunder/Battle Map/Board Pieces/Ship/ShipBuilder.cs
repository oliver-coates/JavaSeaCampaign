using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    #region Initialisation & Destruction
    private void Awake()
    {
        GameMaster.OnReadyForShipSpawn += SpawnShip;
    }

    private void OnDestroy()
    {
        GameMaster.OnReadyForShipSpawn -= SpawnShip;
    }
    #endregion

    private void SpawnShip(Ship ship)
    {
        
    }
}
