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
    [SerializeField] private bool _gameUnderway;
    public bool gameUnderway
    {
        get
        {
            return _gameUnderway;
        }	
    }

    [SerializeField] private List<BoardPiece> _boardPieces;


    private void Awake()
    {
        _boardPieces = new List<BoardPiece>();
        BoardPiece.OnBoardPieceInitialised += InitialiseBoardPiece;
    }

    private void InitialiseBoardPiece(BoardPiece piece)
    {
        Debug.Log($"Recieved a: " + piece.GetType());
        _boardPieces.Add(piece);
    }

    private void Update()
    {
        UpdateDebug();
    }

    private void PlayGame()
    {
        _gameUnderway = true;

        OnTimePlay.Invoke();
    }

    private void PauseGame()
    {
        _gameUnderway = false;

        OnTimePause.Invoke();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_gameUnderway)
            {
                PauseGame();
            }     
            else
            {
                PlayGame();
            }
        }
    }
    #endregion
}
