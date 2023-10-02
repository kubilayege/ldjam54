using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameGrid gameGrid;
    private Vector2Int _currentPosition;
    private bool isInClaimState;
    
    private void Start()
    {
        transform.position = gameGrid.GridElements[0, 0].transform.position + Vector3.up;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
            return;
        }
        
        if(Input.anyKeyDown)
        {
            var direction = (Vector2.right * (Input.GetAxis("Horizontal")) +
                             Vector2.up * (Input.GetAxis("Vertical"))).normalized;

            Debug.Log(direction);
            if (direction.magnitude > 0)
            {
                Move(direction);
            }
        }
    }

    public void Move(Vector2 direction)
    {
        var vector2Int = new Vector2Int((int)direction.x,(int)direction.y);
        var newPosition = _currentPosition + vector2Int;

        Debug.Log(vector2Int);

        if (!gameGrid.IsInGrid(newPosition)) return;
        if (!gameGrid.IsPositionIsAClaim(newPosition) && !isInClaimState)
        {
            transform.position += new Vector3(vector2Int.x, 0f, vector2Int.y);
            _currentPosition = newPosition;

            return;
        }
        if (!isInClaimState)
        {
            gameGrid.InitEnemyClaimMove(this, _currentPosition);
            isInClaimState = true;
        }
        transform.position += new Vector3(vector2Int.x, 0f, vector2Int.y);

        var isClaimOver = gameGrid.TrackEnemyMovement(this, newPosition);

        _currentPosition = newPosition;

        if (isClaimOver) isInClaimState = false;
    }
    
}