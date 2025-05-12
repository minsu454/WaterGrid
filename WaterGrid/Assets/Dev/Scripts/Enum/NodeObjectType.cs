using System;

/// <summary>
/// 연결 오브젝트 타입
/// </summary>
[Flags]
public enum NodeObjectType
{
    None = 0,
    Water = 1 << 0,
    Pump = 1 << 1,
    House = 1 << 2,
}
