using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour, IObjectPoolable<Line>, Interactionable
{
    [SerializeField] private LineRenderer lineRenderer;                     //라인 렌더러
    [SerializeField] private EdgeCollider2D edgeCollider;                   //라인 렌더러

    protected NodeObject parentNodeObject = null;                           //부모 객체 오브젝트
    public NodeObject ParentNodeObject
    {
        get { return parentNodeObject; }
    }

    protected NodeObject childrenNodeObject = null;                         //자식 객체 오브젝트
    public NodeObject ChildrenNodeObject
    {
        get { return childrenNodeObject; }
    }

    private readonly List<Vector2> setPointList = new List<Vector2>(2);     //부모자식 위치 리스트

    public event Action<Line> ReturnEvent;

    /// <summary>
    /// 라인 임시 연결 함수
    /// </summary>
    public void TempConnect(Transform startTr)
    {
        lineRenderer.positionCount = 2;
        SetLinePosition(0, startTr.position);
    }

    /// <summary>
    /// 라인 연결 함수
    /// </summary>
    public void Connect(NodeObject parent, NodeObject children)
    {
        parentNodeObject = parent;
        childrenNodeObject = children;

        OnUpdate(0, parent.transform.position);
        OnUpdate(1, children.transform.position);

        setPointList.Add(parent.transform.position);
        setPointList.Add(children.transform.position);

        edgeCollider.SetPoints(setPointList);
    }

    /// <summary>
    /// 라인 잡는 것 취소 함수
    /// </summary>
    public void Cancel()
    {
        lineRenderer.positionCount = 2;

        OnUpdate(0, parentNodeObject.transform.position);
        OnUpdate(1, childrenNodeObject.transform.position);
    }

    /// <summary>
    /// 업데이트 함수
    /// </summary>
    public void OnUpdate(int idx, Vector2 pos)
    {
        SetLinePosition(idx, pos);
    }

    /// <summary>
    /// 라인 위치 세팅 함수
    /// </summary>
    private void SetLinePosition(int idx, Vector3 pos)
    {
        lineRenderer.SetPosition(idx, pos);
    }

    /// <summary>
    /// 라인 연결 해제 함수
    /// </summary>
    public void Disconnect()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }

    public void Performed()
    {
        lineRenderer.positionCount = 3;
        OnUpdate(2, lineRenderer.GetPosition(1));

        MapManager.Line.SetTempLine(this);
    }

    public void Pressed()
    {
        if (MapManager.Line.GetSelected(out Interactionable interactionable) is false)
            return;

        if (interactionable.Equals(parentNodeObject) is true)
        {
            MapManager.Line.ChangeTempLine(parentNodeObject, childrenNodeObject, childrenNodeObject);
        }
        else if (interactionable.Equals(childrenNodeObject) is true)
        {
            MapManager.Line.ChangeTempLine(parentNodeObject, childrenNodeObject, parentNodeObject);
        }
        else
        {

        }

    }

    public void Canceled()
    {
        MapManager.Line.DeleteTempLine();
    }
}
