using Common.ListEx;
using Common.Pool;
using System.Collections.Generic;
using UnityEngine;

public sealed class LineContainer
{
    private readonly Dictionary<NodeObject, List<(NodeObject key, Line value)>> _lineDict = new();

    private ObjectPool<Line> linePool;
    public Line GetObject
    {
        get { return linePool.GetObject(); }
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init(GameObject linePrefab)
    {
        linePool = new ObjectPool<Line>(nameof(Line), linePrefab, null, 3);
    }

    /// <summary>
    /// 라인 추가 함수
    /// </summary>
    public void Add(NodeObject parent, NodeObject children, Line line)
    {
        if (_lineDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList))
        {
            _lineDict[parent].Add((children, line));
            return;
        }

        tupleList = new List<(NodeObject key, Line value)>
        {
            (children, line)
        };

        _lineDict.Add(parent, tupleList);
    }

    /// <summary>
    /// 라인 삭제 함수
    /// </summary>
    public Line Remove(NodeObject parent, NodeObject children)
    {
        if (_lineDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList) is false)
        {
            Debug.LogError($"Is not found Dictionary Key : _LineDict");
            return default;
        }

        if (tupleList.TryGetTupleByKey(children, out (NodeObject key, Line value) tuple) is false)
        {
            Debug.LogError($"Is not found tupleList Key : (NodeObject, Line)");
            return default;
        }

        parent.OnDisconnectLineParent(children.MyCost);
        children.OnDisconnectLineChildren(parent.MyCost);
        tupleList.Remove(tuple);
        _lineDict[parent] = tupleList;

        if (tupleList.Count == 0)
            _lineDict.Remove(parent);

        return tuple.value;
    }

    /// <summary>
    /// 라인 전부 삭제 함수
    /// </summary>
    public void RemoveAll(NodeObject parent)
    {
        if (_lineDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList) is false)
            return;

        foreach ((NodeObject key, Line value) tuple in tupleList)
        {
            parent.OnDisconnectLineParent(tuple.key.MyCost);
            tuple.key.OnDisconnectLineChildren(parent.MyCost);

            SetConnectTopObject(parent, tuple.key, false);

            tuple.value.Disconnect();
        }

        _lineDict[parent].Clear();
        _lineDict.Remove(parent);
    }

    /// <summary>
    /// 노드가 포함되어 있는지 체크 함수
    /// </summary>
    public bool Contains(NodeObject parent, NodeObject children)
    {
        if (_lineDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList) is false)
        {
            return false;
        }

        if (tupleList.ContainsTuple(children) is false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 노드가 포함되어 있는지 체크 함수
    /// </summary>
    public bool TryGetValue(NodeObject parent, out List<(NodeObject key, Line value)> tupleList)
    {
        if (_lineDict.TryGetValue(parent, out tupleList) is false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 최상위 오브젝트와 연결되어 있는지 설정해주는 함수
    /// </summary>
    public void SetConnectTopObject(NodeObject parent, NodeObject children, bool value)
    {
        children.SetIsConnectTopObject(value);
        if (TryGetValue(children, out List<(NodeObject key, Line value)> tupleList) is false)
            return;

        foreach (var tuple in tupleList)
        {
            SetConnectTopObject(children, tuple.key, value);
        }
    }
}
