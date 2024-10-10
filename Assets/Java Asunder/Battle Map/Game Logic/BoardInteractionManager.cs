using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInteractionManager : MonoBehaviour
{

    [SerializeField] private Camera _camera;

    private void Update()
    {
        if (!GameMaster.BattleUnderway)
        {
            return;
        }

        CheckForInput();
    }

    private void CheckForInput()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1))
        {
            SetShipDestination(mousePosition);
        }
    }

    private void SetShipDestination(Vector3 destination)
    {
        if (GameMaster.SelectedShip == null)
        {
            return;
        }

        if (GameMaster.SelectedShip.isPlayerShip)
        {
            // Cannot set destination for player ship
            // It is manually controlled by the player
            return;
        }

        GameMaster.SelectedShip.instance.AI.SetDestination(destination);
    }
}
