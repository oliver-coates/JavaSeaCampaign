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
        if (_ship == ship)
        {
            // We are already assigned, return
            return;
        }

        // Unsubscribe from old ship events:
        if (_ship != null)
        {
            _ship.OnChange -= RefreshUI;
        }

        // This is a new ship: Go Ahead with reassigning.
        _ship = ship;

        _ship.OnChange += RefreshUI;

        SetupContextualMenu();
        RefreshUI();
    }

    private void OnDestroy()
    {
        _ship.OnChange -= RefreshUI;
    }

    private void RefreshUI()
    {
        _nameText.text = _ship.GetFullName();

        _descriptionText.text = $"{_ship.nation.nationNameDesc} {_ship.shipClass.name} {_ship.shipClass.shipType.name}";
    }

    private void SetupContextualMenu()
    {
        ContextualMenu.Option[] options = new ContextualMenu.Option[3];

        options[0] = new ContextualMenu.Option("Remove", StartRemove);
        options[1] = new ContextualMenu.Option("Change Nation", StartChangeNation);
        options[2] = new ContextualMenu.Option("Rename", StartRename);

        _contextualMenu.Initialise(options);
    }

    private void StartRemove()
    {
        SessionMaster.RemoveShip(_ship);
    }

    private void StartChangeNation()
    {
        ObjectRequestHandler.RequestNationType(_ship.ChangeNation);
    }

    private void StartRename()
    {
        BasicInputManager.RequestInput(new BasicInputManager.InputRequest($"Rename the {_ship.GetFullName()}", _ship.Rename));
    }
}
