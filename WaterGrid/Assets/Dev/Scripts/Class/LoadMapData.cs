using System;
using System.Collections.Generic;

/// <summary>
/// 맵 데이터
/// </summary>
[Serializable]
public class LoadMapData
{
    public string MapName;
    public List<tileData> TileDataList;
}
