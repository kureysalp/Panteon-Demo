using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnGameStart();
    public static event OnGameStart GameStarted;

    public delegate void OnGameWin();
    public static event OnGameWin GameWon;

    public delegate void OnResetGame();
    public static event OnResetGame GameReset;

    public delegate void OnPaintState();
    public static event OnPaintState PaintingState;

    public delegate void OnStartPainting();
    public static event OnStartPainting StartedPainting;

    public delegate void OnEndPainting();
    public static event OnEndPainting EndedPainting;

    public static void StartGame()
    {
        GameStarted?.Invoke();
    }

    public static void WinGame()
    {
        GameWon?.Invoke();
    }

    public static void ResetGame()
    {
        GameReset?.Invoke();
    }

    public static void GoPaintingState()
    {
        PaintingState?.Invoke();
    }

    public static void StartPainting()
    {
        StartedPainting?.Invoke();        
    }

    public static void EndPainting()
    {
        EndedPainting?.Invoke();
    }

}

