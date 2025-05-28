using System;
using UnityEngine;
/// <summary>
/// 위험아이콘을 들고 있는 인터페이스
/// </summary>
public interface IWarningable
{
    public string Name { get { return transform.name; } }

    public Transform transform { get; }

    /// <summary>
    /// 위험 아이콘
    /// </summary>
    public WarningIcon Icon { get; }

    /// <summary>
    /// 아웃 라인
    /// </summary>
    public SpriteOutline Outline { get; }

    /// <summary>
    /// 아웃 라인 알파 값 설정 함수
    /// </summary>
    /// <param name="value"></param>
    public void SetOutLineAlpha(float value);
}
