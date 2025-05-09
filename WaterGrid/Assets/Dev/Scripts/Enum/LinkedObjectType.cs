using System;

/// <summary>
/// 연결 오브젝트 타입
/// </summary>
[Flags]
public enum LinkedObjectType
{
    None = 0,
    House = 1 << 0,
    Water = 1 << 1,
    Pump = 1 << 2,
}
