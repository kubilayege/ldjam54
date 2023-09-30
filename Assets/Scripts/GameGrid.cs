using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameGrid : MonoBehaviour
{
    public GridSettings gridSettings;
    public GridElement[,] GridElements;
    
    private void Awake()
    {
        gridSettings.OnGridUpdated = GenerateGrid; 
        gridSettings.UpdateGridWall = UpdateGridWall; 
        GenerateGrid();
    }

    public void UpdateGridWall(Vector2Int obj)
    {
        GridElements[obj.x, obj.y].UpdateGridWall(GetGridNeighbourData(obj.x, obj.y));

        UpdateGridElements();
    }

    public void GenerateGrid()
    {
        if (gridSettings.gridSize.x < 3 || gridSettings.gridSize.y < 3) return;
        
        if (GridElements != null)
        {
            foreach (var gridElement in GridElements)
            {
                Destroy(gridElement.gameObject);
            }
        }
        
        GridElements = new GridElement[gridSettings.gridSize.x, gridSettings.gridSize.y];

        GenerateFloor();
        GenerateBorder();

        UpdateGridElements();

        transform.position = Vector3.zero + gridSettings.gridOffset;
    }

    private void UpdateGridElements()
    {
        for (var x = 0; x < GridElements.GetLength(0); x++)
        for (var y = 0; y < GridElements.GetLength(1); y++)
        {
            var gridElement = GridElements[x, y];
            if(gridElement.CurrentGridElementState == GridElementState.Floor)
            {
                gridElement.SwitchToFloor();
                continue;
            }
            
            var gridNeighbourData = GetGridNeighbourData(x, y);
            gridElement.UpdateGridWall(gridNeighbourData);
        }
    }

    private void GenerateFloor()
    {
        var index = new Vector2Int(0, 0);
        for (var x = 0; x < GridElements.GetLength(0); x++)
        for (var y = 0; y < GridElements.GetLength(1); y++)
        {
            // if(GridElements[x,y].currentGridSolidType != GridSolidType.Floor) continue;
            
            index.x = x;
            index.y = y;
            GridElements[x, y] = Instantiate(gridSettings.gridElementPrefab, transform);

            var gridElementOptions = new GridElementOptions
            {
                Position = transform.TransformPoint(gridSettings.CalculateGridPosition(index))
            };
            GridElements[x, y].Init(gridElementOptions, GridElementState.Floor, index);
        }
    }

    private void GenerateBorder()
    {
        var gridSize = gridSettings.gridSize;
        var index = new Vector2Int(0, 0);

        for (int i = 0; i < gridSize.y; i++)
        {
            var rowIncrement = i % (gridSize.y - 1) == 0 ? 1 : gridSize.x - 1;   
            for (int j = 0; j < gridSize.x; j += rowIncrement)
            {
                index.x = j;
                index.y = i;
                var position = transform.TransformPoint(gridSettings.CalculateGridPosition(index));
                var gridElementOptions = new GridElementOptions
                {
                    Position = position
                };
                GridElements[j,i].Init(gridElementOptions, GridElementState.Wall, index);
            }
        }
    }

    private int[] GetGridNeighbourData(int x, int y)
    {
        var top = y + 1 < gridSettings.gridSize.y ? GridElements[x, y + 1] != null
            ? GridElements[x, y + 1].CurrentGridElementState != GridElementState.Floor ? 1 : 0
            : 0 : 0;
        var bottom = y-1 >= 0 ? GridElements[x, y - 1] != null
            ? GridElements[x, y - 1].CurrentGridElementState != GridElementState.Floor ? 1 : 0
            : 0 : 0;
        var left = x-1 >= 0 ? GridElements[x - 1, y] != null
            ? GridElements[x - 1, y].CurrentGridElementState != GridElementState.Floor ? 1 : 0
            : 0 : 0;
        var right = x + 1 < gridSettings.gridSize.x ? GridElements[x + 1, y] != null
            ? GridElements[x + 1, y].CurrentGridElementState != GridElementState.Floor? 1 : 0
            : 0 : 0;
        var gridNeighbourData = new int[] { top, bottom, left, right };

        // Debug.Log(gridNeighbourData[09]);
        return gridNeighbourData;
    }

    private void OnDisable()
    {
        gridSettings.OnGridUpdated = null;
        gridSettings.UpdateGridWall = null;
    }

    public bool IsPositionInGrid(Vector2Int newPosition)
    {
        var gridSize = gridSettings.gridSize;
        return newPosition.y < gridSize.y && newPosition.x < gridSize.x && newPosition.x >= 0 && newPosition.y >= 0;
    }
}