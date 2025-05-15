using Common.ListEx;
using System.Collections.Generic;
using UnityEngine;

public sealed class NodeContainer
{
    private readonly Dictionary<NodeObject, List<(NodeObject key, Line value)>> _nodeDict = new Dictionary<NodeObject, List<(NodeObject key, Line value)>>();
    public Dictionary<NodeObject, List<(NodeObject key, Line value)>> NodeDict
    {
        get { return _nodeDict; }
    }

    /// <summary>
    /// 노드 추가 함수
    /// </summary>
    public void Add(NodeObject parent, NodeObject children, Line line)
    {
        if (_nodeDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList))
        {
            _nodeDict[parent].Add((children, line));
            return;
        }

        tupleList = new List<(NodeObject key, Line value)>
        {
            (children, line)
        };

        _nodeDict.Add(parent, tupleList);
    }

    /// <summary>
    /// 노드 삭제 함수
    /// </summary>
    public Line Remove(NodeObject parent, NodeObject children)
    {
        if (_nodeDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList) is false)
        {
            Debug.LogError($"Is not found Dictionary Key : _nodeDict");
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

        if (tupleList.Count == 0)
            _nodeDict.Remove(parent);

        return tuple.value;
    }

    /// <summary>
    /// 노드가 포함되어 있는지 체크 함수
    /// </summary>
    public bool Contains(NodeObject parent, NodeObject children)
    {
        if (_nodeDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList) is false)
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
        if (_nodeDict.TryGetValue(parent, out tupleList) is false)
        {
            return false;
        }

        return true;
    }

}