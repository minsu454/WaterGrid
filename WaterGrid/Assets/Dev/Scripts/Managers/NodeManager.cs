using Common.ListEx;
using Common.PhysicsEx;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Metadata;

public sealed class NodeManager
{
    private readonly Dictionary<NodeObject, List<(NodeObject key, Line value)>> _nodeDict = new Dictionary<NodeObject, List<(NodeObject key, Line value)>>();

    private Line tempLine;                                      //임시 라인
    private Interactionable tempInteractionable;                //선택 
    public Interactionable TempInteractionable
    {
        get { return tempInteractionable; }
    }

    public event Action<int, Vector2> lineUpdateEvent;          //라인위치 업데이트 이벤트

    public void OnUpdate()
    {
        lineUpdateEvent?.Invoke(1, InputManager.inputWorldPoint);
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
        if (tempLine == null || (tempInteractionable is NodeObject) is false)
        {
            CancelLine();
            return;
        }

        NodeObject parent = (NodeObject)tempInteractionable;
        NodeObject children = node;

        if (parent == children || parent.CanConnect(children) is false)
        {
            CancelLine();
            return;
        }

        if (parent.Type >= children.Type)
        {
            children = parent;
            parent = node;
        }

        if (PhysicsEx.IsPointInCircle(parent.transform.position, parent.Radius, children.transform.position) is false
             || Contains(parent, children))
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

        Line line = Remove(children.ParentNodeObject, children);
        line.Unconnect();
    }

    /// <summary>
    /// 노드 삭제 함수
    /// </summary>
    private Line Remove(NodeObject parent, NodeObject children)
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

        parent.OnUnConnectLineParent(children.MyCost);
        children.OnUnConnectLineChildren(parent.MyCost);
        tupleList.Remove(tuple);

        if (tupleList.Count == 0)
            _nodeDict.Remove(parent);

        return tuple.value;
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
    /// 노드 연결 선 제작 함수
    /// </summary>
    public void CreateLine(NodeObject firstObj)
    {
        tempLine = GameManager.instance.lineObjectPool.GetObject();

        tempLine.TempConnect(firstObj.transform);
        lineUpdateEvent += tempLine.OnUpdate;
        tempLine.gameObject.SetActive(true);

        tempInteractionable = firstObj;
    }

    public void SetLine(Line line)
    {
        tempLine = line;
        lineUpdateEvent += tempLine.OnUpdate;

        tempInteractionable = line;
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
        tempInteractionable = null;
    }

    /// <summary>
    /// 라인 연결 해제 함수
    /// </summary>
    public void UnConnectLine(NodeObject parent, NodeObject children)
    {
        Remove(parent, children);
        CancelLine();
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
        tempInteractionable = null;
    }

    /// <summary>
    /// 임시 선으로 변환 함수
    /// </summary>
    public void ChangeTempLine(NodeObject curParent, NodeObject children, NodeObject tempParent)
    {
        Remove(curParent, children);
        tempInteractionable = tempParent;
        tempLine.TempConnect(tempParent.transform);
    }

    /// <summary>
    /// 레이쏴서 블록에 맞은 것을 저장해주는 함수
    /// </summary>
    public bool GetSelected(out Interactionable node)
    {
        node = null;

        if (IsMouseHit(out RaycastHit2D hit) is false)
            return false;

        if (hit.collider.TryGetComponent(out node) is false)
            return false;

        return true;
    }

    /// <summary>
    /// 마우스 클릭방향으로 레이를 쏴서 spawnpoint가 있는지 체크하는 함수 
    /// </summary>
    private bool IsMouseHit(out RaycastHit2D hit)
    {
        Vector2 vec = InputManager.inputWorldPoint;
        hit = Physics2D.Linecast(vec, vec * 5);

        if (!hit)
            return false;

        return true;
    }
}
