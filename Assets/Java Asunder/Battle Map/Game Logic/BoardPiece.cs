using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardPiece : MonoBehaviour
{
    public static event Action<BoardPiece> OnBoardPieceInitialised;
    public static event Action<BoardPiece> OnBoardPieceDestroyed;

    protected bool _gameUnderway = false;


    #region Initialisation & Destruction
    private void Start()
    {
        GameMaster.OnTimePlay += TimePlay;
        GameMaster.OnTimePause += TimePause;

        OnBoardPieceInitialised.Invoke(this);
        Initialise();
    }

    private void OnDestroy()
    {
        GameMaster.OnTimePause -= TimePause;
        GameMaster.OnTimePlay -= TimePlay;

        OnBoardPieceDestroyed?.Invoke(this);
    }
    #endregion

    private void Update()
    {
        if (_gameUnderway)
        {
            GameTick();
        }

        UpdateTick();
    }

    private void TimePause()
    {
        _gameUnderway = false;
    }

    private void TimePlay()
    {
        _gameUnderway = true;
    }


    /// <summary>
    /// Called when this board piece is created.
    /// </summary>
    protected abstract void Initialise();

    /// <summary>
    /// Called every frame
    /// </summary>
    protected abstract void UpdateTick();

    /// <summary>
    /// Called every frame in which the game is unpaused.
    /// </summary>
    protected abstract void GameTick();

}
