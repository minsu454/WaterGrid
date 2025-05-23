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
    public int Weight;
    public TileType TileType;

    public TileData(Vector2Int position, int weight, TileType type)
    {
        Position = position;
        Weight = weight;
        TileType = type;
    }

    public TileData(KeyValuePair<Vector2Int, (int weight, TileType type)> pair)
    {
        Position = pair.Key;
        Weight = pair.Value.weight;
        TileType = pair.Value.type;
    }
}