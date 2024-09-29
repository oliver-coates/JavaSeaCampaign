using System.Collections;
using System.Collections.Generic;
using KahuInteractive.UIHelpers;
using Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipInstanceUI : MonoBehaviour
{
    private readonly Color INACTIVE_COLOR = new Color(0.75f, 0.75f, 0.75f, 0.75f);
    private readonly Color ACTIVE_COLOR = new Color(1f, 1f, 1f, 1f);

    private Ship _ship;

    [Header("UI References:")]
    public RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private ContextualMenuLocation _contextualMenu;
    [SerializeField] private Image _flagImage;

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
        _flagImage.sprite = _ship.nation.flag;

        _descriptionText.text = $"{_ship.nation.nationNameDesc} {_ship.shipClass.name} {_ship.shipClass.shipType.name}";

        if (_ship.isIncludedInBattle)
        {
            _flagImage.color = ACTIVE_COLOR;
            _nameText.color = ACTIVE_COLOR;
            _descriptionText.color = ACTIVE_COLOR;
        }
        else
        {
            _flagImage.color = INACTIVE_COLOR;
            _nameText.color = INACTIVE_COLOR;
            _descriptionText.color = INACTIVE_COLOR;
        }
    }

    private void SetupContextualMenu()
    {
        ContextualMenu.Option[] options = new ContextualMenu.Option[4];

        options[0] = new ContextualMenu.Option("Remove", StartRemove);
        options[1] = new ContextualMenu.Option("Change Nation", StartChangeNation);
        options[2] = new ContextualMenu.Option("Rename", StartRename);
        options[3] = new ContextualMenu.Option("Set Active/Inactive", StartAddToBattle);

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

    private void StartAddToBattle()
    {
        _ship.SetIncludedInBattle(!_ship.isIncludedInBattle);
    }
}
