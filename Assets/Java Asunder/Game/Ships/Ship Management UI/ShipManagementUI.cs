 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ships;
using KahuInteractive.HassleFreeSaveLoad;

public class ShipManagementUI : MonoBehaviour
{
    [SerializeField] private GameObject _UIPrefab;
    private List<ShipInstanceUI> _shipUIs;

    [SerializeField] private RectTransform _contentZoneRectTransform;

    private void Awake()
    {
        _shipUIs = new List<ShipInstanceUI>();
        SessionMaster.OnShipsChanged += DrawShipUI;
    }

    private void OnDestroy()
    {
        SessionMaster.OnShipsChanged -= DrawShipUI;
    } 

    private void DrawShipUI()
    {
        foreach (ShipInstanceUI shipUI in _shipUIs)
        {
            Destroy(shipUI.gameObject);
        }
        _shipUIs = new List<ShipInstanceUI>();

        int shipUIIndex = 0;
        foreach (Ship ship in SessionMaster.ships)
        {
            ShipInstanceUI shipUI = Instantiate(_UIPrefab, _contentZoneRectTransform.transform).GetComponent<ShipInstanceUI>();

            shipUI.AssignToShip(ship);

            Vector2 pos = new Vector2(0, -40 * (shipUIIndex * 70));
            shipUI.rectTransform.anchoredPosition = pos;

            shipUIIndex++;
        }
    }

    public void CreateNewShip()
    {
        ObjectRequestHandler.RequestShipClass(CreateNewShip);
    }

    public void CreateNewShip(ShipClassType newShipClass)
    {
        Debug.Log($"Creating new ship of type: {newShipClass.name}");

        // Create a new ship from provided type
        Ship newShip = SaveLoad.InstantiateSerializedObject<Ship>("Ship In Play");
        SessionMaster.AddShip(newShip);
    }
}
