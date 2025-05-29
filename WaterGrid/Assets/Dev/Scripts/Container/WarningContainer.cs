using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public sealed class WarningContainer
{
    private readonly Dictionary<string, List<IWarningable>> _warningableDict = new();        //위험 아이콘이 붙은 리스트

    public List<IWarningable> this[string name]
    {
        get
        {
            if (ContainsKey(name) is false)
            {
                Debug.LogError($"Is not Found Dictionary name : {name}");
                return default;
            }

            return _warningableDict[name];
        }
    }

    /// <summary>
    /// 리스트에 추가 함수
    /// </summary>
    public void Add(string name, IWarningable warningable)
    {
        if (TryGetValue(name, out List<IWarningable> warningableList) is true)
        {
            _warningableDict[name].Add(warningable);
            return;
        }

        _warningableDict.Add(name, new List<IWarningable>() { warningable });
    }

    /// <summary>
    /// 리스트에 삭제 함수
    /// </summary>
    public void Remove(string name, IWarningable warningable)
    {
        if (TryGetValue(name, out List<IWarningable> warningableList) is false || warningableList.Contains(warningable) is false)
        {
            return;
        }

        warningableList.Remove(warningable);
        _warningableDict[name] = warningableList;

        if (warningableList.Count == 0)
            _warningableDict.Remove(name);
    }

    /// <summary>
    /// 리스트에 검색 함수
    /// </summary>
    public bool ContainsKey(string name)
    {
        return _warningableDict.ContainsKey(name);
    }

    /// <summary>
    /// 리스트에 검색 시도 및 반환 함수
    /// </summary>
    public bool TryGetValue(string name, out List<IWarningable> warningableList)
    {
        return _warningableDict.TryGetValue(name, out warningableList);
    }

    /// <summary>
    /// Queue에 DeQueue 구현 함수
    /// </summary>
    public IWarningable DeQueue(string name)
    {
        if (ContainsKey(name) is false)
        {
            Debug.LogError($"Is not Found Dictionary name : {name}");
            return default;
        }

        IWarningable warningable = _warningableDict[name][0];
        _warningableDict[name].Remove(warningable);
        _warningableDict[name].Add(warningable);
        return warningable;
    }

    /// <summary>
    /// 리스트에 아웃라인 설정 함수
    /// </summary>
    public void SetOutLines(float value)
    {
        foreach (KeyValuePair<string, List<IWarningable>> warningablePair in _warningableDict)
        {
            for (int i = warningablePair.Value.Count - 1; i >= 0; i--)
            {
                warningablePair.Value[i].SetOutLineAlpha(value);
            }
        }
    }
}