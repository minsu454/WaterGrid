using System;
using UnityEngine;
/// <summary>
/// 에리어 데이터
/// </summary>
[Serializable]
public class AreaData
{
    public string Name;
    public Color Color;
    public int Weight;

    public AreaData(string name, Color color, int weight = 1)
    {
        Name = name;
        Color = color;
        Weight = weight;
    }
}