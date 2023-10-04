using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameGrid : SingletonBehaviour<GameGrid>
{
    public GridSettings gridSettings;
    public GridElement[,] GridElements;

    public List<EnemyMoveData> EnemyMoveDatas;
    public List<EnemyMoveData> currentKnownBorders;

    public Action<float> OnGridUpdated;

    protected override void Awake()
    {
        gridSettings.OnGridUpdated = GenerateGrid;
        GenerateGrid();
    }

    public void UpdateGridWall(Vector2Int index, bool updateAll = false)
    {
        GridElements[index.x, index.y].UpdateGridWall(GetGridNeighbourData(index.x, index.y));

        if(updateAll)
            UpdateGridElements();
    }

    public bool TrackEnemyMovement(Enemy enemy, Vector2Int index)
    {
        var gridElement = GridElements[index.x, index.y];
        var enemyData = EnemyMoveDatas.Find(data => enemy == data.enemy);

        if (gridElement.CurrentGridElementState == GridElementState.Wall)
        {
            Profiler.BeginSample("ClaimAreaSearch");
            enemyData.currentElement.Add(gridElement);
            FinishEnemyMoveData(enemyData);
            var claimAreaWalls = FindClaimArea(enemyData);
            Profiler.EndSample();

            Profiler.BeginSample("ClaimWalls");

            for (var i = 0; i < claimAreaWalls.Count; i++)
            {
                var claimArea = claimAreaWalls[i];
                UpdateGridWall(claimArea.index);
            }
            
            Profiler.EndSample();

            
            Profiler.BeginSample("ClaimCleanupLeftOver");

            CleanupLeftOverWalls();
            
            Profiler.EndSample();
            
            
            Profiler.BeginSample("ClaimUpdateAll");

            UpdateGridElements();

            Profiler.EndSample();

            EnemyMoveDatas.Remove(enemyData);
            return true;
        }

        enemyData.currentElement.Add(gridElement);

        gridElement.TempWall();

        return false;
    }

    private void CleanupLeftOverWalls()
    {
        for (var x = 0; x < GridElements.GetLength(0); x++)
        for (var y = 0; y < GridElements.GetLength(1); y++)
        {
            var gridElement = GridElements[x, y];
            var neighbours = GetNeighbourGridElements(gridElement.index);
            var isIsolated = true;
            for (var index = 0; index < neighbours.Length; index++)
            {
                var neighbour = neighbours[index];
                if (neighbour == null) continue;
                if (neighbour.CurrentGridElementState != GridElementState.Wall) isIsolated = false;
            }

            if (isIsolated)
            {
                UpdateGridWall(gridElement.index);
            }
        }
    }

    private List<GridElement> FindClaimArea(EnemyMoveData enemyData)
    {
        var index = enemyData.currentElement[^1].index;
        var neighbours = GetNeighbourGridElements(index);


        if (neighbours[0] != null && neighbours[1] != null)
        {
            var isNeighboursBorder = neighbours[0].IsBorder && neighbours[1];
            if (isNeighboursBorder || (!enemyData.currentElement.Contains(neighbours[0]) &&
                                       !enemyData.currentElement.Contains(neighbours[1])))
            {
                return FindClaimAreaFloodFill(neighbours[0], neighbours[1], enemyData);
            }
        }


        if (neighbours[2] != null && neighbours[3] != null)
        {
            var isNeighboursBorder = neighbours[2].IsBorder && neighbours[3].IsBorder;

            if (isNeighboursBorder || (!enemyData.currentElement.Contains(neighbours[2]) &&
                                       !enemyData.currentElement.Contains(neighbours[3])))
            {
                return FindClaimAreaFloodFill(neighbours[2], neighbours[3], enemyData);
            }
        }

        Debug.Log("No Flood Fill Area Was Found?");

        return null;
    }

    private List<GridElement> FindClaimAreaFloodFill(GridElement first, GridElement second, EnemyMoveData enemyData)
    {
        var floodFillResult = new List<GridElement>();
        FloodFill(ref floodFillResult, first, enemyData.currentElement);
        floodFillResult = floodFillResult.PickFloors();


        var floodFillResult2 = new List<GridElement>();
        FloodFill(ref floodFillResult2, second, enemyData.currentElement);
        floodFillResult2 = floodFillResult2.PickFloors();


        // If the start and end claim walls are neighbours and border of the grid floodfill has a bug. 
        foreach (var gridElement in floodFillResult)
        {
            if (floodFillResult2.Contains(gridElement))
            {
                floodFillResult2.Remove(gridElement);
            }
        }

        floodFillResult.AddRange(enemyData.currentElement);
        floodFillResult2.AddRange(enemyData.currentElement);
        List<GridElement> result;

        if (floodFillResult.Count > floodFillResult2.Count)
        {
            result = floodFillResult2;
            // if (IsPlayerInFloodFill(floodFillResult2))
            //     result = floodFillResult;
        }
        else
        {
            result = floodFillResult;

            // if (IsPlayerInFloodFill(floodFillResult))
            //     result = floodFillResult;
        }

        return result;
    }

    // private bool IsPlayerInFloodFill(List<GridElement> floodFillResult)
    // {
    //     return floodFillResult.Contains(new GridElement());
    // }

    private bool FloodFill(ref List<GridElement> floodFillResult, GridElement first,
        List<GridElement> border)
    {
        var neighbours = GetNeighbourGridElements(first.index);

        foreach (var neighbour in neighbours)
        {
            if (neighbour == null) continue;

            if (!border.Contains(neighbour) && !floodFillResult.Contains(neighbour))
            {
                floodFillResult.Add(neighbour);
                FloodFill(ref floodFillResult, neighbour, border);
            }
        }

        return true;
    }

    private GridElement[] GetNeighbourGridElements(Vector2Int index)
    {
        var neighbours = new GridElement[4];

        var topPosition = index.Top();
        var downPosition = index.Down();
        var leftPosition = index.Left();
        var rightPosition = index.Right();
        neighbours[0] = IsInGrid(topPosition) ? GridElements[topPosition.x, topPosition.y] : null;
        neighbours[1] = IsInGrid(downPosition) ? GridElements[downPosition.x, downPosition.y] : null;
        neighbours[2] = IsInGrid(leftPosition) ? GridElements[leftPosition.x, leftPosition.y] : null;
        neighbours[3] = IsInGrid(rightPosition) ? GridElements[rightPosition.x, rightPosition.y] : null;

        return neighbours;
    }

    private void FinishEnemyMoveData(EnemyMoveData enemyMoveData)
    {
        var tempBorders = new List<List<GridElement>>();
        var enemyStartGrid = enemyMoveData.currentElement[0];
        var enemyEndGrid = enemyMoveData.currentElement[^1];

        if (enemyStartGrid.IsBorder && enemyEndGrid.IsBorder)
        {
            currentKnownBorders.Add(enemyMoveData);
            return;
        }

        if (!enemyStartGrid.IsBorder)
        {
            var listAllBorders = new List<EnemyMoveData>();
            foreach (var currentKnownBorder in currentKnownBorders)
            {
                if (!currentKnownBorder.currentElement.Contains(enemyStartGrid))
                {
                    continue;
                }

                listAllBorders.Add(currentKnownBorder);
            }

            listAllBorders.Sort((data, data2) => (data.currentElement.Count > data2.currentElement.Count) ? 1 : 0);

            var selected = listAllBorders[0];

            var index = selected.currentElement.IndexOf(enemyStartGrid);
            if (index != selected.currentElement.Count - 1)
            {
                tempBorders.Add(selected.currentElement.GetRange(index + 1, selected.currentElement.Count - 1 - index));
            }

            tempBorders.Add(selected.currentElement.GetRange(0, index));


            foreach (var tempBorder in tempBorders)
            {
                tempBorder.AddRange(enemyMoveData.currentElement);
            }
        }
        else
        {
            tempBorders.Add(enemyMoveData.currentElement);
        }

        var finalTempBorders = new List<List<GridElement>>();

        if (!enemyEndGrid.IsBorder)
        {
            var listAllBorders = new List<EnemyMoveData>();
            foreach (var currentKnownBorder in currentKnownBorders)
            {
                if (!currentKnownBorder.currentElement.Contains(enemyEndGrid))
                {
                    continue;
                }

                listAllBorders.Add(currentKnownBorder);
            }

            listAllBorders.Sort((data, data2) => (data.currentElement.Count > data2.currentElement.Count) ? 1 : 0);

            var selected = listAllBorders[0];

            foreach (var tempBorder in tempBorders)
            {
                var index = selected.currentElement.IndexOf(enemyEndGrid);
                if (index != selected.currentElement.Count - 1)
                {
                    var list = (selected.currentElement.GetRange(index + 1, selected.currentElement.Count - 1 - index));
                    var newTempBorder = new List<GridElement>();
                    newTempBorder.AddRange(tempBorder);
                    newTempBorder.AddRange(list);
                    finalTempBorders.Add(newTempBorder);
                }

                var border = new List<GridElement>();
                var list2 = selected.currentElement.GetRange(0, index);
                border.AddRange(tempBorder);
                border.AddRange(list2);
                finalTempBorders.Add(border);
            }
        }
        else
        {
            tempBorders.Sort((list, list2) => list.Count > list2.Count ? 1 : 0);
            // tempBorders[0].AddRange(enemyMoveData.currentElement);
            finalTempBorders.Add(tempBorders[0]);
        }

        finalTempBorders.Sort((list, list2) => list.Count < list2.Count ? 1 : 0);
        enemyMoveData.currentElement = finalTempBorders[0];
        currentKnownBorders.Add(enemyMoveData);
    }

    private void OnDrawGizmos()
    {
        if (currentKnownBorders == null) return;
        currentKnownBorders.Sort((data, moveData) => data.currentElement.Count > moveData.currentElement.Count ? 1 : 0);
        foreach (var currentKnownBorder in currentKnownBorders)
        {
            System.Random random = new System.Random(currentKnownBorder.GetHashCode());
            Gizmos.color = new Color(random.Next(0, 100) / 100f, random.Next(0, 100) / 100f,
                random.Next(0, 100) / 100f);
            foreach (var gridElement in currentKnownBorder.currentElement)
            {
                Gizmos.DrawSphere(gridElement.transform.position + Vector3.up, 0.1f);
            }
        }
    }

    // private (GridElement left, GridElement right) FindLeftRightGridElement(EnemyMoveData enemyMoveData)
    // {
    //     foreach (var gridElement in enemyMoveData.currentElement)
    //     {
    //         var gridNeighbourData = GetGridNeighbourData(gridElement.index.x, gridElement.index.y);
    //
    //         var rightElement = GridElements[gridElement.index.x + 1, gridElement.index.y];
    //         var leftElement = GridElements[gridElement.index.x + 1, gridElement.index.y];
    //         if(!enemyMoveData.currentElement.Contains(rightElement))
    //     }
    // }

    public void GenerateGrid()
    {
        EnemyMoveDatas = new List<EnemyMoveData>();

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
        var floorCount = 0;
        for (var x = 0; x < GridElements.GetLength(0); x++)
        for (var y = 0; y < GridElements.GetLength(1); y++)
        {
            var gridElement = GridElements[x, y];
            if (gridElement.CurrentGridElementState == GridElementState.Floor)
            {
                gridElement.SwitchToFloor();
                floorCount++;
                continue;
            }

            var gridNeighbourData = GetGridNeighbourData(x, y);
            gridElement.UpdateGridWall(gridNeighbourData);
        }

        var totalGridElementCount = gridSettings.gridSize.x * gridSettings.gridSize.y;
        var borderCount = 2f * gridSettings.gridSize.x + 2f * gridSettings.gridSize.y - 4;
        var percentage = 1f - floorCount / (totalGridElementCount - borderCount);
        
        //OnGridUpdated.Invoke(percentage);
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
                GridElements[j, i].Init(gridElementOptions, GridElementState.Wall, index, true);
            }
        }
    }

    private int[] GetGridNeighbourData(int x, int y)
    {
        var top = y + 1 < gridSettings.gridSize.y
            ? GridElements[x, y + 1] != null
                ? GridElements[x, y + 1].CurrentGridElementState != GridElementState.Floor ? 1 : 0
                : 0
            : 0;
        var bottom = y - 1 >= 0
            ? GridElements[x, y - 1] != null
                ? GridElements[x, y - 1].CurrentGridElementState != GridElementState.Floor ? 1 : 0
                : 0
            : 0;
        var left = x - 1 >= 0
            ? GridElements[x - 1, y] != null
                ? GridElements[x - 1, y].CurrentGridElementState != GridElementState.Floor ? 1 : 0
                : 0
            : 0;
        var right = x + 1 < gridSettings.gridSize.x
            ? GridElements[x + 1, y] != null
                ? GridElements[x + 1, y].CurrentGridElementState != GridElementState.Floor ? 1 : 0
                : 0
            : 0;
        var gridNeighbourData = new int[] { top, bottom, left, right };

        // Debug.Log(gridNeighbourData[09]);
        return gridNeighbourData;
    }

    private void OnDisable()
    {
        gridSettings.OnGridUpdated = null;
    }

    public bool IsInGrid(Vector2Int newPosition)
    {
        var gridSize = gridSettings.gridSize;
        var isEmptyPositionInGrid = (newPosition.y < gridSize.y && newPosition.x < gridSize.x &&
                                     newPosition is { x: >= 0, y: >= 0 });
        return isEmptyPositionInGrid;
    }

    public void InitEnemyClaimMove(Enemy enemy, Vector2Int currentPosition)
    {
        var enemyMoveData = new EnemyMoveData()
        {
            enemy = enemy,
            currentElement = new List<GridElement>()
        };

        var neighbourData = GetGridNeighbourData(currentPosition.x, currentPosition.y);

        enemyMoveData.currentElement.Add(GridElements[currentPosition.x, currentPosition.y]);
        EnemyMoveDatas.Add(enemyMoveData);
    }

    public bool IsPositionIsAClaim(Vector2Int newPosition)
    {
        return GridElements[newPosition.x, newPosition.y].CurrentGridElementState == GridElementState.Floor;
    }

    public void Reset()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        currentKnownBorders = new List<EnemyMoveData>();
        EnemyMoveDatas = new List<EnemyMoveData>();
        GenerateGrid();
    }
}

[Serializable]
public class EnemyMoveData
{
    public Enemy enemy;
    public List<GridElement> currentElement;
    public GridElement firstFloodFillArea;
    public GridElement secondFloodFillArea;
}