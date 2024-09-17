using System.Collections;
using System.Collections.Generic;
using KahuInteractive.UIHelpers;
using Ships;
using TMPro;
using UnityEngine;

public class ShipInstanceUI : MonoBehaviour
{
    private Ship _ship;

    [Header("UI References:")]
    public RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private ContextualMenuLocation _contextualMenu;

    public void AssignToShip(Ship ship)
    {
        _ship = ship;
    
        RefreshUI();
    }

    private void RefreshUI()
    {
        _nameText.text = _ship.GetFullName();

        _descriptionText.text = $"{_ship.nation.nationNameDesc} {_ship.shipClass.shipType.typeName}";
    }


}
