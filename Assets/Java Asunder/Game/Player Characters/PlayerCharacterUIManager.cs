using System.Collections;
using System.Collections.Generic;
using KahuInteractive.HassleFreeSaveLoad;
using KahuInteractive.UIHelpers;
using UnityEngine;
using System;

public class PlayerCharacterUIManager : MonoBehaviour
{
    private static event Action OnPlayerCharactersUpdated;


    [Header("Settings:")]
    [SerializeField] private ContentDistributor.Direction _contentDirection;

    [Header("State:")]
    [SerializeField] private List<PlayerCharacterUI> _playerCharacterUIs;

    [Header("References:")]
    [SerializeField] private GameObject _playerCharacterUIPrefab;
    [SerializeField] private RectTransform _contentZone;

    private void Awake()
    {
        SessionMaster.OnLoaded += RefreshUI;
        OnPlayerCharactersUpdated += RefreshUI;

        _playerCharacterUIs = new List<PlayerCharacterUI>();
    }

    private void OnDestroy()
    {
        SessionMaster.OnLoaded -= RefreshUI;
        OnPlayerCharactersUpdated -= RefreshUI;
    }

    public void CreateNewPlayerCharacter()
    {
        PlayerCharacter newPlayerCharacter = SaveLoad.InstantiateSerializedObject<PlayerCharacter>("Player Character");

        SessionMaster.playerCharacters.Add(newPlayerCharacter);

        OnPlayerCharactersUpdated?.Invoke();
    }

    private void RefreshUI()
    {
        // Delete any old player character UIs
        DeleteExistingElements();

        // Create new ones, assigning the players character to them
        foreach (PlayerCharacter character in SessionMaster.playerCharacters)
        {
            PlayerCharacterUI createdUI = Instantiate(_playerCharacterUIPrefab, _contentZone.transform).GetComponent<PlayerCharacterUI>();

            // createdUI.transform.localScale = Vector3.one;
            // createdUI.transform.SetParent(transform);

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
            Destroy(_playerCharacterUIs[i].gameObject);
        }
        _playerCharacterUIs = new List<PlayerCharacterUI>();
    }

    private void PositionElements()
    {   
        List<RectTransform> contents = new List<RectTransform>();
        
        foreach (PlayerCharacterUI characterUI in _playerCharacterUIs)
        {
            contents.Add(characterUI.GetComponent<RectTransform>());
        }

        ContentDistributor.DistributeContent(contents, _contentZone, _contentDirection);
    }
}
