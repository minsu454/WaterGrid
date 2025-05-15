using Common.PhysicsEx;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Metadata;

public sealed class NodeManager
{
    private NodeContainer _container = new NodeContainer();

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
    /// 노드를 추가 조건 검사 및 추가 함수
    /// </summary>
    public void TryAdd(NodeObject node)
    {
        if (tempLine == null || (tempInteractionable is NodeObject) is false)
        {
            DeleteLine();
            return;
        }

        NodeObject parent = (NodeObject)tempInteractionable;
        NodeObject children = node;

        if (parent == children || parent.CanConnect(children) is false)
        {
            DeleteLine();
            return;
        }

        if (parent.Type >= children.Type)
        {
            children = parent;
            parent = node;
        }

        if ((PhysicsEx.IsPointInCircle(parent.transform.position, parent.Radius, children.transform.position) is false
            && parent.Type != children.Type)
             || _container.Contains(parent, children))
        {
            DeleteLine();
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

        Line line = _container.Remove(children.ParentNodeObject, children);
        line.Disconnect();
    }

    /// <summary>
    /// 최상위 오브젝트와 연결되어 있는지 설정해주는 함수
    /// </summary>
    private void SetConnectTopObject(NodeObject parent, NodeObject children, bool value)
    {
        children.SetIsConnectTopObject(value);
        if (_container.TryGetValue(children, out List<(NodeObject key, Line value)> tupleList) is false)
            return;

        foreach (var tuple in tupleList)
        {
            SetConnectTopObject(children, tuple.key, value);
        }
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

    /// <summary>
    /// 라인 클릭시 라인 잡고 있는 함수
    /// </summary>
    public void SetLine(Line line)
    {
        tempLine = line;
        lineUpdateEvent += tempLine.OnUpdate;

        tempInteractionable = line;
    }

    /// <summary>
    /// 라인 선택 취소 함수
    /// </summary>
    public void CancelLine(Line line)
    {
        line.Cancel();
        lineUpdateEvent -= tempLine.OnUpdate;
        tempLine = null;
        tempInteractionable = null;
    }

    /// <summary>
    /// 라인 연결 함수
    /// </summary>
    public void ConnectLine(NodeObject parent, NodeObject children)
    {
        tempLine.Connect(parent, children);
        _container.Add(parent, children, tempLine);

        if (parent.IsConnectTopObject is true)
        {
            SetConnectTopObject(parent, children, true);
        }

        parent.OnConnectLineChildren(children.MyCost);
        children.OnConnectLineParent(parent);

        lineUpdateEvent -= tempLine.OnUpdate;
        tempLine = null;
        tempInteractionable = null;
    }

    /// <summary>
    /// 라인 연결 해제 함수
    /// </summary>
    public void DisconnectLine(NodeObject parent, NodeObject children)
    {
        _container.Remove(parent, children);
        SetConnectTopObject(parent, children, false);
        DeleteLine();
    }

    /// <summary>
    /// 임시 선 삭제 함수
    /// </summary>
    public void DeleteLine()
    {
        if (tempLine == null)
            return;

        tempLine.Disconnect();
        lineUpdateEvent -= tempLine.OnUpdate;
        tempLine = null;
        tempInteractionable = null;
    }

    /// <summary>
    /// 임시 선으로 변환 함수
    /// </summary>
    public void ChangeTempLine(NodeObject curParent, NodeObject children, NodeObject tempParent)
    {
        _container.Remove(curParent, children);
        SetConnectTopObject(curParent, children, false);
        tempInteractionable = tempParent;
        tempLine.TempConnect(tempParent.transform);
    }

    /// <summary>
    /// 레이쏴서 블록에 맞은 것을 반환해주는 함수
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
