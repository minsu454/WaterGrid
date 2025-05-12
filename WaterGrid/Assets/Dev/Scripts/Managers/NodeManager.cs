using Common.ListEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
        if (tempNodeObject == node)
        {
            CancelLine();
            return;
        }

        NodeObject parent = tempNodeObject;
        NodeObject children = node;
        if (parent.Type > children.Type)
        {
            parent = node;
            children = tempNodeObject;
        }

        if (parent.CanConnect(children) is false)
        {
            CancelLine();
            return;
        }

        lineUpdateEvent?.Invoke(1, node.transform.position);
        Add(parent, children, tempLine);
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

        if (tupleList.TryGetTuple(children, out (NodeObject, Line) tuple) is false)
        {
            Debug.LogError($"Is not found tupleList Key : (NodeObject, Line)");
            return;
        }

        tupleList.Remove(tuple);

        if (tupleList.Count == 0)
            _nodeDict.Remove(parent);
    }

    /// <summary>
    /// 노드 연결 선 제작 함수
    /// </summary>
    public void CreateLine(NodeObject firstObj)
    {
        tempLine = GameManager.instance.lineObjectPool.GetObject();

        tempLine.Link(firstObj.transform);
        lineUpdateEvent += tempLine.OnUpdate;
        tempLine.gameObject.SetActive(true);

        tempNodeObject = firstObj;
    }

    public void ConnectLine()
    {

    }

    /// <summary>
    /// 임시 선 삭제 함수
    /// </summary>
    public void CancelLine()
    {
        if (tempLine == null)
            return;

        lineUpdateEvent -= tempLine.OnUpdate;
        tempLine.Unlink();
        tempLine = null;
    }

    public void DeleteLine()
    {

    }
}
