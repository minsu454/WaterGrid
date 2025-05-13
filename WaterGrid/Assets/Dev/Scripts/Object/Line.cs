using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour, IObjectPoolable<Line>, Interactionable
{
    [SerializeField] private LineRenderer lineRenderer;             //라인 렌더러
    [SerializeField] private EdgeCollider2D edgeCollider;           //라인 렌더러

    protected NodeObject parentNodeObject = null;                   //부모 객체 오브젝트
    public NodeObject ParentNodeObject
    {
        get { return parentNodeObject; }
    }

    public event Action<Line> ReturnEvent;

    /// <summary>
    /// 라인 임시 연결 함수
    /// </summary>
    public void TempConnect(Transform startTr)
    {
        SetLinePosition(0, startTr.position);
    }

    /// <summary>
    /// 라인 연결 함수
    /// </summary>
    public void Connect(NodeObject parent, NodeObject children)
    {
        parentNodeObject = parent;
        OnUpdate(0, parent.transform.position);
        OnUpdate(1, children.transform.position);

        edgeCollider.SetPoints(new List<Vector2>() { parent.transform.position, children.transform.position });
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
    public void Unconnect()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }

    public void Performed()
    {
        Debug.Log("Preformed");
    }

    public void Canceled()
    {
        Debug.Log("Canceled");
    }
}
