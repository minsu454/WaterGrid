using System;
using System.Collections.Generic;

/// <summary>
/// 맵 데이터
/// </summary>
[Serializable]
public class LoadMapData : ILoadDatable
{
    public string Name;
    public int width;
    public int height;
    public List<TileData> TileDataList;

    public bool IsValid()
    {
        return Name == null || Name == "";
    }
}
