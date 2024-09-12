using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterUI : MonoBehaviour
{
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
    }

    private void Update()
    {
        if (_playerCharacter == null)
        {
            return;
        }

        _completionBarImage.fillAmount = (_playerCharacter.taskTimer / _playerCharacter.timeToCompleteTask);
    }
}
