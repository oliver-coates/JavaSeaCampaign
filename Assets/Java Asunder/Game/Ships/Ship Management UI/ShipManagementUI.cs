 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ships;

public class ShipManagementUI : MonoBehaviour
{
    [SerializeField] private GameObject _UIPrefab;

    [SerializeField] private RectTransform _contentZoneRectTransform;

    private void Awake()
    {
        SessionMaster.OnShipsChanged += DrawShipUI;
    }

    private void OnDestroy()
    {
        SessionMaster.OnShipsChanged -= DrawShipUI;
    } 

    private void DrawShipUI()
    {

    }

    public void CreateNewShip()
    {
        ObjectRequestHandler.RequestShipClass(CreateNewShip);
    }

    public void CreateNewShip(ShipClassType newShip)
    {
        // Create a new ship from provided type

        Debug.Log($"Creating new ship of type: {newShip.name}");
    }
}
