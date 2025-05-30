using System;
using System.Collections.Generic;

/// <summary>
/// 맵 데이터
/// </summary>
[Serializable]
public class MapData : ILoadDatable
{
    public string Name;

    public int Width;
    public int Height;

    public Bounds2D Bounds;

    public int totalValue;

    public List<TileData> TileDataList;
    public List<AreaData> AreaDataList;

    public bool IsValid()
    {
        return Name == null || Name == "";
    }
}
