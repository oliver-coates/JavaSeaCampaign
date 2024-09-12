using System.Collections;
using System.Collections.Generic;
using KahuInteractive.HassleFreeSaveLoad;
using UnityEngine;

public class PlayerCharacterUIManager : MonoBehaviour
{

    [Header("State:")]
    [SerializeField] private List<PlayerCharacterUI> _playerCharacterUIs;

    [Header("References:")]
    [SerializeField] private GameObject _playerCharacterUIPrefab;

    private void Awake()
    {
        SessionMaster.OnLoaded += RefreshUI;
        _playerCharacterUIs = new List<PlayerCharacterUI>();
    }

    public void CreateNewPlayerCharacter()
    {
        PlayerCharacter newPlayerCharacter = SaveLoad.InstantiateSerializedObject<PlayerCharacter>("Player Character");

        SessionMaster.playerCharacters.Add(newPlayerCharacter);
    }

    private void RefreshUI()
    {
        // Delete any old player character UIs
        DeleteExistingElements();

        // Create new ones, assigning the players character to them
        foreach (PlayerCharacter character in SessionMaster.playerCharacters)
        {
            PlayerCharacterUI createdUI = Instantiate(_playerCharacterUIPrefab, transform).GetComponent<PlayerCharacterUI>();
            createdUI.AssignToPlayerCharacter(character); 
            _playerCharacterUIs.Add(createdUI);
        }

        // Position them inside here
        PositionElements();
    }

    private void DeleteExistingElements()
    {
        for (int i = 0; i < _playerCharacterUIs.Count; i++)
        {
            Destroy(_playerCharacterUIs[i]);
        }
        _playerCharacterUIs = new List<PlayerCharacterUI>();
    }

    private void PositionElements()
    {

    }
}
