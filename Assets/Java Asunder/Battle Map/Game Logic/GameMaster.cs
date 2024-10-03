using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    #region Game Event Callbacks
    public static event Action OnBattleStart;
    public static event Action OnBattleEnd;
    public static event Action OnTimePause;
    public static event Action OnTimePlay;
    #endregion

    [Header("Game State:")]

    // Is there a battle currently underway
    [SerializeField] private bool _battleUnderway;
    public bool battleUnderway
    {
        get
        {
            return _battleUnderway;
        }	
    }

    // Is time paused?
    [SerializeField] private bool _battlePaused;
    public bool battlePaused
    {
        get
        {
            return _battlePaused;
        }	
    }


    [SerializeField] private List<BoardPiece> _boardPieces;


    #region Initialistion
    private void Awake()
    {
        _boardPieces = new List<BoardPiece>();
        BoardPiece.OnBoardPieceInitialised += AddBoardPiece;
        BoardPiece.OnBoardPieceDestroyed += RemoveBoardPiece;

    }

    private void OnDestroy()
    {
        BoardPiece.OnBoardPieceInitialised -= AddBoardPiece;
        BoardPiece.OnBoardPieceDestroyed -= RemoveBoardPiece;
    }



    #endregion

    #region Board Pieces
    private void AddBoardPiece(BoardPiece piece)
    {
        _boardPieces.Add(piece);
    }

    private void RemoveBoardPiece(BoardPiece piece)
    {
        _boardPieces.Remove(piece);
    }
    #endregion

    #region Battle start & end

    public void StartBattle()
    {
        // Delete all old peices
        ClearBoard();

        // Ensure the game starts paused.
        PauseBattle();

        _battleUnderway = true;
        OnBattleStart?.Invoke();
    }

    public void EndBattle()
    {
        _battleUnderway = false;
        OnBattleEnd?.Invoke();
    }

    /// <summary>
    /// Destroys all board peices
    /// </summary>
    private void ClearBoard()
    {
        foreach (BoardPiece piece in _boardPieces)
        {
            Destroy(piece);
        }
    }

    #endregion

    #region Battle time control
    private void PlayBattle()
    {
        if (_battleUnderway == false)
        {
            Debug.Log($"Attempting to pause a battle which is not running");
            return;
        }
        _battlePaused = false;

        OnTimePlay.Invoke();
    }

    private void PauseBattle()
    {
        if (_battleUnderway == false)
        {
            Debug.Log($"Attempting to play a battle which is not running");
            return;
        }
        _battlePaused = true;

        OnTimePause.Invoke();
    }
    #endregion


    #region Debug

    private void Update()
    {
        UpdateDebug();
    }

    private void UpdateDebug()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            CheckForDebugCommand();
        }
    }

    private void CheckForDebugCommand()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _battleUnderway)
        {
            if (_battlePaused)
            {
                PlayBattle();
            }     
            else
            {
                PauseBattle();
            }
        }
    }
    #endregion
}
