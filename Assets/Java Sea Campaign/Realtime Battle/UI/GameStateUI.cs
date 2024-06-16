using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private Image _vignette;
    [SerializeField] private Image _turnTimeIndicator;


    private void Awake()
    {
        _vignette.CrossFadeAlpha(0, 0.01f, true);        
    
        GameMaster.onTurnStart += TurnStart;
        GameMaster.onTurnEnd += TurnEnd;
    }

    private void Update()
    {
        float lerp = GameMaster.turnTimer / GameMaster.TURN_TIME;
        lerp = Mathf.Clamp(lerp, 0, 1);

        _turnTimeIndicator.fillAmount = 1 - lerp;
    }


    private void TurnStart()
    {
        _vignette.CrossFadeAlpha(1, 0.5f, true);
    }

    private void TurnEnd()
    {
        _vignette.CrossFadeAlpha(0, 0.5f, true);
        _turnTimeIndicator.fillAmount = 0f;
    }
}
