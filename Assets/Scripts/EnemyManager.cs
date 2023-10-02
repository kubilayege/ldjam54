using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : SingletonBehaviour<EnemyManager>
{
    // TODO:  manage enemies, Move and Claim, Spawn Based on game settings.
    [SerializeField] private List<Enemy> enemies;
    
    
    public void OnEnemyKilled(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            RoundManager.Instance.RoundWon();
        }
    }
    
}