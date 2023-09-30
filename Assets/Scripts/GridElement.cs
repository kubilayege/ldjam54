﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum GridElementState
{
    Floor,
    Wall,
    ClaimInProgress
}

public enum GridSolidType
{
    FullWall = 0,
    NarrowWall = 1,
    SideWall = 2,
    CornerWall = 3,
    Floor = 4,
    Claim = 5
}

public partial class GridElement : MonoBehaviour
{
    public List<GameObject> gridElementModels;
    public GridSolidType currentGridSolidType;
    public GridElementState CurrentGridElementState;
    public Vector2Int index;
    
    public void Init(GridElementOptions gridElementOptions, GridElementState gridElementState, Vector2Int gridIndex)
    {
        transform.position = gridElementOptions.Position;
        CurrentGridElementState = gridElementState;
        index = gridIndex;
    }

    public void UpdateGridWall(int[] gridElements)
    {
        // Debug.Log($"{gridElements[0]} {gridElements[1]} {gridElements[2]} {gridElements[3]}");

        foreach (var gridElementLookUp in GridElementNeighbourLookup)
        {
            var hasMatched = gridElementLookUp.Key.IsNeighbourDataMatch(gridElements);
            if (!hasMatched) continue;

            var gridSolidType = gridElementLookUp.Value;
            var rotation = gridElementLookUp.Key.GetRotation(gridSolidType);

            transform.rotation = rotation;
            
            SwitchModel((int)gridSolidType);

            currentGridSolidType = gridSolidType;

            CurrentGridElementState = GridElementState.Wall;

            return;
        }

        Debug.Log("No Match");
    }

    public void SwitchToFloor()
    {
        SwitchModel(4);

        currentGridSolidType = GridSolidType.Floor;
    }

    private void SwitchModel(int index)
    {
        foreach (var gridElementModel in gridElementModels)
        {
            gridElementModel.SetActive(false);
        }

        gridElementModels[index].SetActive(true);
    }
}

public struct GridElementOptions
{
    public Vector3 Position;
}

public struct GridElementNeighbourData
{
    public int[] neighbourData;

    public bool IsNeighbourDataMatch(int[] data)
    {
        for (int i = 0; i < 4; i++)
        {
            if (neighbourData[i] != data[i]) return false;
        }

        return true;
    }

    public Quaternion GetRotation(GridSolidType gridSolidType)
    {
        switch (gridSolidType)
        {
            case GridSolidType.FullWall:
                return Quaternion.identity;
                break;
            case GridSolidType.NarrowWall:
                if (neighbourData[0]==1)
                {
                    return Quaternion.Euler(0f, 90f, 0f);
                }
                
                return Quaternion.identity;
                break;
            case GridSolidType.SideWall:
                var direction = new Vector3(neighbourData[2]-neighbourData[3], 0f, neighbourData[1] - neighbourData[0]); 
                return Quaternion.LookRotation(direction, Vector3.up);
                break;
            case GridSolidType.CornerWall:
                return Quaternion.identity;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gridSolidType), gridSolidType, null);
        }
    }
}

