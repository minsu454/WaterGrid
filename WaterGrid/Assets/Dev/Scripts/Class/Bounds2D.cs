using System;
using UnityEngine;

/// <summary>
/// 사각형 Vector 저장용
/// </summary>
[Serializable]
public struct Bounds2D
{
    public Vector2 min;
    public Vector2 max;

    public Bounds2D(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }

    public Vector3 Center => (min + max) * 0.5f;
    public Vector3 Size => new Vector3(max.x - min.x, max.y - min.y, 0);
}
