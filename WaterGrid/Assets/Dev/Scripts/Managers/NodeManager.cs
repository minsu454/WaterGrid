using Common.ListEx;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public sealed class NodeManager
{
    private readonly Dictionary<NodeObject, List<(NodeObject key, Line value)>> _nodeDict = new Dictionary<NodeObject, List<(NodeObject key, Line value)>>();

    private Line tempLine;                                      //임시 라인
    private NodeObject tempNodeObject;                          //선택 노드

    public event Action<int, Vector2> lineUpdateEvent;          //라인위치 업데이트 이벤트

    public void OnUpdate()
    {
        lineUpdateEvent?.Invoke(1, InputManager.MousePoint);
    }

    /// <summary>
    /// 노드 추가 함수
    /// </summary>
    private void Add(NodeObject parent, NodeObject children, Line line)
    {
        if (_nodeDict.TryGetValue(parent, out List <(NodeObject key, Line value)> tupleList))
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
    /// 노드를 추가 조건 검사 및 추가 함수
    /// </summary>
    public void TryAdd(NodeObject node)
    {
        NodeObject parent = tempNodeObject;
        NodeObject children = node;

        if (parent == children || parent.CanConnect(children) is false)
        {
            CancelLine();
            return;
        }

        if (parent.Type > children.Type)
        {
            parent = node;
            children = tempNodeObject;
        }

        if (Contains(parent, children))
        {
            CancelLine();
            return;
        }

        HaveParentInChildren(children);

        ConnectLine(parent, children);
    }

    /// <summary>
    /// 이미 부모노드에 연결되어 있었을 경우 함수
    /// </summary>
    private void HaveParentInChildren(NodeObject children)
    {
        if (children.ParentNodeObject == null)
            return;

        Remove(children.ParentNodeObject, children);
    }

    /// <summary>
    /// 노드 삭제 함수
    /// </summary>
    private void Remove(NodeObject parent, NodeObject children)
    {
        if (_nodeDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList) is false)
        {
            Debug.LogError($"Is not found Dictionary Key : _nodeDict");
            return;
        }

        if (tupleList.TryGetTuple(children, out (NodeObject key, Line value) tuple) is false)
        {
            Debug.LogError($"Is not found tupleList Key : (NodeObject, Line)");
            return;
        }

        parent.OnUnConnectLine(children.MyCost);
        tuple.value.Unconnect();
        tupleList.Remove(tuple);

        if (tupleList.Count == 0)
            _nodeDict.Remove(parent);
    }

    /// <summary>
    /// 노드가 포함되어 있는지 체크 함수
    /// </summary>
    private bool Contains(NodeObject parent, NodeObject children)
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
    /// 라인이 포함되어 있는지 체크 후 반환 함수
    /// </summary>
    private bool TryGetLine(NodeObject parent, NodeObject children, out Line line)
    {
        line = default;

        if (_nodeDict.TryGetValue(parent, out List<(NodeObject key, Line value)> tupleList) is false)
        {
            return false;
        }

        if (tupleList.TryGetTuple(children, out (NodeObject key, Line value) result) is false)
        {
            return false;
        }

        line = result.value;
        return true;
    }

    /// <summary>
    /// 노드 연결 선 제작 함수
    /// </summary>
    public void CreateLine(NodeObject firstObj)
    {
        tempLine = GameManager.instance.lineObjectPool.GetObject();

        tempLine.TempConnect(firstObj.transform);
        lineUpdateEvent += tempLine.OnUpdate;
        tempLine.gameObject.SetActive(true);

        tempNodeObject = firstObj;
    }

    /// <summary>
    /// 라인 연결 함수
    /// </summary>
    public void ConnectLine(NodeObject parent, NodeObject children)
    {
        tempLine.Connect(parent, children);
        Add(parent, children, tempLine);

        parent.OnConnectLineChildren(children.MyCost);
        children.OnConnectLineParent(parent);

        lineUpdateEvent -= tempLine.OnUpdate;
        tempLine = null;
        tempNodeObject = null;
    }

    /// <summary>
    /// 임시 선 삭제 함수
    /// </summary>
    public void CancelLine()
    {
        if (tempLine == null)
            return;

        tempLine.Unconnect();
        lineUpdateEvent -= tempLine.OnUpdate;
        tempLine = null;
        tempNodeObject = null;
    }

    public void DeleteLine()
    {

    }
}
