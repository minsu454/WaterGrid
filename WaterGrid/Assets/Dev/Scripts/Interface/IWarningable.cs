using System;
using UnityEngine;
/// <summary>
/// Addressable로 데이터 받는 친구들 인터페이스
/// </summary>
public interface IWarningable
{
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
