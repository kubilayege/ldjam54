using System.Collections.Generic;
using UnityEngine;

public static class GridHelper
{
    public static Vector2Int Top(this Vector2Int vector)
    {
        var newVector = vector;
        newVector.y += 1;
        return newVector;
    }
    
    public static Vector2Int Down(this Vector2Int vector)
    {
        var newVector = vector;
        newVector.y -= 1;
        return newVector;
    }
    
    public static Vector2Int Left(this Vector2Int vector)
    {
        var newVector = vector;
        newVector.x -= 1;
        return newVector;
    }
    
    public static Vector2Int Right(this Vector2Int vector)
    {
        var newVector = vector;
        newVector.x += 1;
        return newVector;
    }

    public static List<GridElement> PickFloors(this List<GridElement> gridElements)
    {
        return gridElements.FindAll(element => element.CurrentGridElementState == GridElementState.Floor);
    }
}