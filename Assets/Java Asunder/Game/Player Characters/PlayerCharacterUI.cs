using System;
using System.Collections;
using System.Collections.Generic;
using KahuInteractive.UIHelpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterUI : MonoBehaviour
{

    [SerializeField] private ContextualMenuLocation _contextualMenu;


    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _taskText;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private Image _completionBarImage;

    private PlayerCharacter _playerCharacter;


    public void AssignToPlayerCharacter(PlayerCharacter playerCharacter)
    {
        _playerCharacter = playerCharacter;

        _nameText.text = playerCharacter.name;
        // _portraitImage.sprite = playerCharacter.image;

        UpdateCompletionBar();

        // Set up the contextual menu:
        ContextualMenu.Option[] options = new ContextualMenu.Option[5];

        options[0] = new ContextualMenu.Option("Remove", Remove);
        options[1] = new ContextualMenu.Option("Rename", Rename);
        
        options[2] = new ContextualMenu.Option("Disable/Enable", SetDisabled);
        options[3] = new ContextualMenu.Option("Set task", SetTask);
        options[4] = new ContextualMenu.Option("Set idle", SetIdle);

        _contextualMenu.Initialise(options);
    }

    private void Update()
    {
        if (_playerCharacter == null)
        {
            return;
        }

        UpdateCompletionBar();
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

    private void Rename()
    {
        Debug.Log($"Rename");
    }

    private void Remove()
    {
        Debug.Log($"Remove");
    }

    private void SetDisabled()
    {
        Debug.Log($"Unconcious");
    }

    private void SetTask()
    {
        Debug.Log($"Task");
    }

    private void SetIdle()
    {
        Debug.Log($"Idle");
    }
}
