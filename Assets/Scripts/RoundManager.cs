using UnityEngine;

public class RoundManager : SingletonBehaviour<RoundManager>
{
    // TODO: manage rounds, Shop round, game round. based on round settings
    [SerializeField] private ScriptableObject[] rounds;
    
    
    protected override void Awake()
    {
        base.Awake();
        
        GameGrid.Instance.OnGridUpdated += OnGridUpdated;
    }

    private void OnGridUpdated(float gridUpdateCount)
    {
        // TODO: get value from game settings 
        if (gridUpdateCount > 0.7f)
        {
            GameManager.Instance.Lose(new LoseData());
        }
    }

    public void RoundWon()
    {
        // 
        if (rounds.Length == 0)
        {
            GameManager.Instance.Win(new WinData());
            return;
        }
        
        GameManager.Instance.AdvanceRound();
    }

    public void NextRound()
    {
        
    }
}