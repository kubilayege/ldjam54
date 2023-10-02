using System;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public Action<LoseData> OnLose;

    public Action<WinData> OnWin;
    // TODO: Manage game states, on win lose conditions 

    public void Lose(LoseData loseData)
    {
        Debug.Log("GAME OVER!");
        OnLose.Invoke(loseData);
    }

    public void AdvanceRound()
    {
        RoundManager.Instance.NextRound();
    }
    
    public void Win(WinData winData)
    {
        Debug.Log("YOU WON!");
        OnWin.Invoke(winData);
    }
}


public struct LoseData
{
    
}

public struct WinData
{
    
}