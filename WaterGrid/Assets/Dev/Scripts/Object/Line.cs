using System;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour, IObjectPoolable<Line>
{
    [SerializeField] private LineRenderer lineRenderer;             //라인 렌더러
    private List<Transform> PointTr = new List<Transform>();        //연결되어 있는 오브젝트 Tr

    public event Action<Line> ReturnEvent;

    /// <summary>
    /// 라인 연결 함수
    /// </summary>
    public void Link(Transform startTr)
    {
        SetLinePosition(0, startTr.position);
        PointTr.Add(startTr);
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
    public void SetLinePosition(int idx, Vector3 pos)
    {
        lineRenderer.SetPosition(idx, pos);
    }

    /// <summary>
    /// 라인 세팅 함수
    /// </summary>
    private void Hold()
    {
        if (PointTr.Count == 0)
            return;

        for (int i = 0; i < PointTr.Count; i++)
        {
            lineRenderer.SetPosition(i, PointTr[i].position);
        }
    }

    /// <summary>
    /// 라인 연결 해제 함수
    /// </summary>
    public void Unlink()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 리셋 함수
    /// </summary>
    public void ResetObject()
    {
        PointTr.Clear();
    }

    private void OnDisable()
    {
        ResetObject();
        ReturnEvent.Invoke(this);
    }
}
