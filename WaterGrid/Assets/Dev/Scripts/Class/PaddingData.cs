using System;
using UnityEngine;

/// <summary>
/// 패딩 데이터(UI Grid Layout)
/// </summary>
[Serializable]
public struct PaddingData
{
    public float Left;
    public float Right;
    public float Top;
    public float Bottom;

    public RectOffset ToRectOffset()
    {
        return new RectOffset(
            Mathf.RoundToInt(Left),
            Mathf.RoundToInt(Right),
            Mathf.RoundToInt(Top),
            Mathf.RoundToInt(Bottom)
        );
    }
}