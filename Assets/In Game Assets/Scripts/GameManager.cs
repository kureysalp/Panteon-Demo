using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{    

    [Header("UI Elements")]
    public GameObject playAgainBtn;
    

    private void Start()
    {        
        EventManager.GameWon += GameWon;
    }    

    private void GameWon()
    {
        playAgainBtn.SetActive(true);
    }

    public void PlayAgain()
    {
        EventManager.ResetGame();
    }
}
