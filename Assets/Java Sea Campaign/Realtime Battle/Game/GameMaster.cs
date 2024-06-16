using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static event Action onTurnStart;
    public static event Action onTurnEnd;

    private static GameMaster _instance;


    public const float TURN_TIME = 5f;

    [SerializeField] private float _turnTimer;
    public static float turnTimer
    {
        get
        {
            return _instance._turnTimer;
        }	
    }

    [SerializeField] private bool _turnUnderway;
    public static bool turnUnderway
    {
        get
        {
            return _instance._turnUnderway;
        }	
    }


    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        UpdateDebug();

        _turnTimer -= Time.deltaTime;
        _turnTimer = Mathf.Clamp(_turnTimer, 0, TURN_TIME);
        if (_turnUnderway && _turnTimer == 0)
        {
            EndTurn();
        }
    }

    private void StartTurn()
    {
        _turnUnderway = true;
        _turnTimer = TURN_TIME;

        onTurnStart?.Invoke();
    }

    private void EndTurn()
    {
        _turnUnderway = false;
        onTurnEnd?.Invoke();
    }

    #region Debug
    private void UpdateDebug()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            CheckForDebugCommand();
        }
    }

    private void CheckForDebugCommand()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartTurn();            
        }
    }
    #endregion
}
