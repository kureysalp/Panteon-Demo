using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnGameStart();
    public static event OnGameStart GameStarted;

    public delegate void OnGameWin();
    public static event OnGameWin GameWon;

    public delegate void OnObstacleHit();
    public static event OnObstacleHit ObstacleHit;

    public delegate void OnResetGame();
    public static event OnResetGame GameReset;


    public static void StartGame()
    {
        GameStarted?.Invoke();
    }

    public static void WinGame()
    {
        GameWon?.Invoke();
    }

    public static void HitObstacle()
    {
        ObstacleHit?.Invoke();
    }

    public static void ResetGame()
    {
        GameReset?.Invoke();
    }


}

