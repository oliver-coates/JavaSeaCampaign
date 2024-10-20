using System;
using System.Collections;
using System.Collections.Generic;
using KahuInteractive.HassleFreeSaveLoad;
using KahuInteractive.UIHelpers;
using Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterUI : MonoBehaviour
{

    private static event Action onPlayerCharacterChanged;

    [SerializeField] private ContextualMenuLocation _contextualMenu;


    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private Image _completionBarImage;

    private PlayerCharacter _playerCharacter;


    #region Unity Methods
    private void Start()
    {
        SetupContextualMenu();
        onPlayerCharacterChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        onPlayerCharacterChanged -= RefreshUI;

        if (_playerCharacter != null)
        {
            _playerCharacter.OnStateChanged -= RefreshUI;
        }
    }

    private void Update()
    {
        if (_playerCharacter == null)
        {
            return;
        }

        UpdateCompletionBar();
    }
    #endregion

    public void AssignToPlayerCharacter(PlayerCharacter playerCharacter)
    {
        _playerCharacter = playerCharacter;
        _playerCharacter.OnStateChanged += RefreshUI;

        RefreshUI();
        UpdateCompletionBar();
    }

    #region Button callbacks:
    private void Rename()
    {
        BasicInputManager.RequestInput(new BasicInputManager.InputRequest("Rename Character", Rename));
    }

    private void Rename(string newName)
    {
        _playerCharacter.characterName = newName;

        onPlayerCharacterChanged?.Invoke();
    }

    private void Remove()
    {
        SessionMaster.RemovePlayerCharacter(_playerCharacter);
    }

    private void SetDisabled()
    {
        _playerCharacter.isDisabled = !_playerCharacter.isDisabled;        

        onPlayerCharacterChanged?.Invoke();
    }

    private void SetTask()
    {
        if (GameMaster.BattleUnderway == false)
        {
            Debug.Log($"Cannot set tasks when battle is not underway");
            return;
        }

        // Get all component slots in the ship:
        ComponentSlot[] slots = SessionMaster.PlayerShip.instance.componentSlots;

        // Set up the category 
        BasicSelection.Category<ComponentSlot> movementDestinations = new BasicSelection.Category<ComponentSlot>();
        movementDestinations.categoryName = "Move Locations";
        
        // Collate all ship component slots into selectable options
        BasicSelection.Option<ComponentSlot>[] options = new BasicSelection.Option<ComponentSlot>[slots.Length];
        for (int slotIndex = 0; slotIndex < slots.Length; slotIndex++)
        {
            ComponentSlot slot = slots[slotIndex];
            options[slotIndex] = new BasicSelection.Option<ComponentSlot>(slot.slotName, slot);
        }
        movementDestinations.options = options;

        // Request the slection, SetMoveTo will be called once an option is decided.
        BasicSelection.RequestSelection<ComponentSlot>(movementDestinations, SetMoveTo);    
    }

    private void SetMoveTo(ComponentSlot destinationSlot)
    {
        _playerCharacter.StartMoveTo(destinationSlot);
    }

    private void SetIdle()
    {
        Debug.Log($"Idle");
    }


    #endregion

    #region UI

    private void SetupContextualMenu()
    {
        // Set up the contextual menu:
        ContextualMenu.Option[] options = new ContextualMenu.Option[5];

        options[0] = new ContextualMenu.Option("Remove", Remove);
        options[1] = new ContextualMenu.Option("Rename", Rename);
        
        options[2] = new ContextualMenu.Option("Disable/Enable", SetDisabled);
        options[3] = new ContextualMenu.Option("Set task", SetTask);
        options[4] = new ContextualMenu.Option("Set idle", SetIdle);

        _contextualMenu.Initialise(options);
    }

    private void RefreshUI()
    {
        _nameText.text = _playerCharacter.characterName;
        // _portraitImage.sprite = _playerCharacter.image;

        if (_playerCharacter.isDisabled)
        {
            _canvasGroup.alpha = 0.25f;
            _taskText.text = "";
        }
        else
        {
            _canvasGroup.alpha = 1f;
            _taskText.text = _playerCharacter.currentTaskString;
        }
    }
    
    private void UpdateCompletionBar()
    {
        // Set fill amount - preventing divide by 0
        if (_playerCharacter.timeToCompleteTask == 0)
        {
            _completionBarImage.fillAmount = 0;
        }
        else
        {
            _completionBarImage.fillAmount = (_playerCharacter.taskTimer / _playerCharacter.timeToCompleteTask);
        }
    }


    #endregion
}

