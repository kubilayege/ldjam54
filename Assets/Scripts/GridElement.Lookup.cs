using System.Collections.Generic;
using UnityEngine;

public partial class GridElement
{
    public Dictionary<GridElementNeighbourData, GridSolidType> GridElementNeighbourLookup =
        new()
        {
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 1, 1, 1 }
                }, GridSolidType.FullWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 0, 1, 1 }
                }, GridSolidType.NarrowWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 1, 0, 0 }
                }, GridSolidType.NarrowWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 0, 0, 1 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 1, 0, 1 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 0, 1, 0 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 1, 1, 0 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 0, 0, 0 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 0, 0, 0 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 1, 0, 0 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 0, 1, 0 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 0, 0, 1 }
                }, GridSolidType.CornerWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 0, 1, 1, 1 }
                }, GridSolidType.SideWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 0, 1, 1 }
                }, GridSolidType.SideWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 1, 0, 1 }
                }, GridSolidType.SideWall
            },
            {
                new GridElementNeighbourData
                {
                    neighbourData = new[] { 1, 1, 1, 0 }
                }, GridSolidType.SideWall
            },
            
        };
}