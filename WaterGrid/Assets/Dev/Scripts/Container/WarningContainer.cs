using System.Collections.Generic;

public sealed class WarningContainer
{
    private readonly List<IWarningable> _warningableList = new List<IWarningable>();        //위험 아이콘이 붙은 리스트

    /// <summary>
    /// 리스트에 추가 함수
    /// </summary>
    public void Add(IWarningable warningable)
    {
        if (Contains(warningable) is true)
            return;

        _warningableList.Add(warningable);
    }

    /// <summary>
    /// 리스트에 삭제 함수
    /// </summary>
    public void Remove(IWarningable warningable)
    {
        if (Contains(warningable) is false)
            return;

        _warningableList.Remove(warningable);
    }

    /// <summary>
    /// 리스트에 검색 함수
    /// </summary>
    public bool Contains(IWarningable warningable)
    {
        return _warningableList.Contains(warningable);
    }

    /// <summary>
    /// 리스트에 아웃라인 설정 함수
    /// </summary>
    public void SetOutLines(float value)
    {
        for (int i = _warningableList.Count - 1; i >= 0; i--)
        {
            _warningableList[i].SetOutLineAlpha(value);
        }
    }
}