using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Create GridSettings", fileName = "GridSettings", order = 0)]
public class GridSettings : ScriptableObject
{
    public Vector2Int gridSize;
    public float gridScale;
    public GridElement gridElementPrefab;
    public Action OnGridUpdated;
    public Action<Vector2Int> UpdateGridWall;
    public Vector3Int gridOffset;

    public CameraSetting cameraSetting;

    private void OnValidate()
    {
        if (OnGridUpdated != null)
        {
            OnGridUpdated.Invoke();
        }
    }

    public Vector3 CalculateGridPosition(Vector2Int index)
    {
        return new Vector3(index.x * gridScale, 0f, index.y * gridScale);
    }
}

[Serializable]
public class CameraSetting
{
    public Vector3 position;
    public Vector3 rotation;
    public float fov;
}