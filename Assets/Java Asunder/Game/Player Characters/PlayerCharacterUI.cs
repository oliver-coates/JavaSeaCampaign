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
    private ComponentEffectiveness _componentCurrentlyBeingRolledToBuff;

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

    private void SetMove()
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

    private void ChooseSetTask()
    {
        if (GameMaster.BattleUnderway == false)
        {
            Debug.Log($"Cannot set tasks when battle is not underway");
            return;
        }

        if (_playerCharacter.location.componentInstance == null)
        {
            // This component slot has no component
            Debug.Log($"Cannot assign task: Slot has no component");
            return; 
        }

        ComponentEffectiveness[] possibleBuffs = _playerCharacter.location.componentInstance.GetComponentEffectivenesses();
        if (possibleBuffs.Length == 0)
        {
            Debug.Log($"Cannot assign task: Slot has nothing to buff");
            return;
        }

        // Set up the category
        BasicSelection.Category<ComponentEffectiveness> componentsToBuff = new BasicSelection.Category<ComponentEffectiveness>();
        componentsToBuff.categoryName = "Components to Buff";

        // Collate all componentEffectiveness into selectable options
        BasicSelection.Option<ComponentEffectiveness>[] options = new BasicSelection.Option<ComponentEffectiveness>[possibleBuffs.Length];
        for (int buffIndex = 0; buffIndex < possibleBuffs.Length; buffIndex++)
        {
            ComponentEffectiveness component = possibleBuffs[buffIndex];
            options[buffIndex] = new BasicSelection.Option<ComponentEffectiveness>(component.name, component);
        }
        componentsToBuff.options = options; 

        BasicSelection.RequestSelection(componentsToBuff, RollForSetTask);
    }

    private void RollForSetTask(ComponentEffectiveness componentToRollToBuff)
    {
        BasicInputManager.InputRequest inputRequest = new BasicInputManager.InputRequest("Roll", SetTask); 
        BasicInputManager.RequestInput(inputRequest);

        if (_componentCurrentlyBeingRolledToBuff != null)
        {
            Debug.LogError("Component is already being rolled for. Aborting/.");
            return;
        }
        else
        {
            _componentCurrentlyBeingRolledToBuff = componentToRollToBuff;
        }
    }

    private void SetTask(string rollAsString)
    {  
        if (int.TryParse(rollAsString, out int roll))
        {
            _componentCurrentlyBeingRolledToBuff.ApplyPlayerBuff(roll);
            string description = _componentCurrentlyBeingRolledToBuff.taskDescription;
            _playerCharacter.StartTask(description, EffectivenessModifier.MODIFIER_TIME);
            _componentCurrentlyBeingRolledToBuff = null;
        }
        else
        {
            Debug.LogError($"Could not parse input {rollAsString} to int");
        }
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
        ContextualMenu.Option[] options = new ContextualMenu.Option[6];

        options[0] = new ContextualMenu.Option("Remove", Remove);
        options[1] = new ContextualMenu.Option("Rename", Rename);
        
        options[2] = new ContextualMenu.Option("Disable/Enable", SetDisabled);
        options[3] = new ContextualMenu.Option("Move", SetMove);
        options[4] = new ContextualMenu.Option("Do task", ChooseSetTask);
        options[5] = new ContextualMenu.Option("Set idle", SetIdle);

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

