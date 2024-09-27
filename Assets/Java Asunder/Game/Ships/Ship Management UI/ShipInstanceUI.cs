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

        _ship.OnDeath += DeleteMe;
        _ship.OnChange += RefreshUI;

        SetupContextualMenu();
        RefreshUI();
    }

    private void DeleteMe()
    {
        _ship.OnDeath -= DeleteMe;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _ship.OnChange -= RefreshUI;
    }

    private void RefreshUI()
    {
        _nameText.text = _ship.GetFullName();

        _descriptionText.text = $"{_ship.nation.nationNameDesc} {_ship.shipClass.name}";
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

    }

    private void StartRename()
    {
        BasicInputManager.RequestInput(new BasicInputManager.InputRequest($"Rename the {_ship.GetFullName()}", _ship.Rename));
    }
}
