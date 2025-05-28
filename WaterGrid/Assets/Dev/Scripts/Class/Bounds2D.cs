using UnityEngine;
/// <summary>
/// 사각형 Vector 저장용
/// </summary>
public struct Bounds2D
{
    public Vector2 min;
    public Vector2 max;

    public Bounds2D(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
    }
}
