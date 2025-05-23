using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 타일 데이터
/// </summary>
[Serializable]
public class TileData
{
    public Vector2Int Position;
    public int AreaIdx;
    public TileType TileType;

    public TileData(Vector2Int position, int areaIdx, TileType type)
    {
        Position = position;
        AreaIdx = areaIdx;
        TileType = type;
    }

    public TileData(KeyValuePair<Vector2Int, (int areaIdx, TileType type)> pair)
    {
        Position = pair.Key;
        AreaIdx = pair.Value.areaIdx;
        TileType = pair.Value.type;
    }
}
