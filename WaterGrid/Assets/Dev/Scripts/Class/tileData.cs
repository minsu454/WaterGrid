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
    public float Fill;
    public TileType TileType;

    public TileData(Vector2Int position, float fill, TileType type)
    {
        Position = position;
        Fill = fill;
        TileType = type;
    }

    public TileData(KeyValuePair<Vector2Int, (float fill, TileType type)> pair)
    {
        Position = pair.Key;
        Fill = pair.Value.fill;
        TileType = pair.Value.type;
    }
}