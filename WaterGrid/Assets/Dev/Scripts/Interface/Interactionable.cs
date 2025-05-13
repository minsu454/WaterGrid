using System;

public interface Interactionable
{
    /// <summary>
    /// 클릭 시 상호작용 함수
    /// </summary>
    public void Performed();

    /// <summary>
    /// 클릭 중 상호작용 함수
    /// </summary>
    public void Pressed();

    /// <summary>
    /// 클릭 해제 상호작용 함수
    /// </summary>
    public void Canceled();
}