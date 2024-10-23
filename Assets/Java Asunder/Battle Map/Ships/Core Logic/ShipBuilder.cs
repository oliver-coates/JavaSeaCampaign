using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

public class ShipBuilder : MonoBehaviour
{
    private readonly Vector3 ENEMY_SPAWN_POSITION = new Vector3(0, 500, 0);
    private readonly Vector3 ENEMY_SPAWN_OFFSET = new Vector3(50, 0, 0); 

    private int numShipsSpawned;

    #region Initialisation & Destruction
    private void Awake()
    {
        GameMaster.OnReadyForShipSpawn += SpawnShip;
        GameMaster.OnBattleStart += ResetShipSpawnedNumber;
    }

    private void OnDestroy()
    {
        GameMaster.OnReadyForShipSpawn -= SpawnShip;
        GameMaster.OnBattleStart -= ResetShipSpawnedNumber;
    }

    #endregion

    private void ResetShipSpawnedNumber()
    {
        numShipsSpawned = 0;
    }

    private void SpawnShip(Ship ship)
    {
        ShipInstance newShip = Instantiate(ship.shipClass.prefab, transform).GetComponent<ShipInstance>();

        newShip.Setup(ship);
         
        Vector3 spawnPosition;
        if (newShip.shipData == SessionMaster.PlayerShip)
        {
            // Player ship starts at 0,0.0
            spawnPosition = Vector3.zero;
        }
        else
        {
            spawnPosition = ENEMY_SPAWN_POSITION + (ENEMY_SPAWN_OFFSET * numShipsSpawned);
            numShipsSpawned++;
        }

        newShip.transform.localPosition = spawnPosition;
        newShip.transform.localRotation = Quaternion.identity;

    }
}
