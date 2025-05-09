using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedObject : MonoBehaviour
{
    private int maxCount;
    private List<Line> _lineList = new List<Line>();
    private Line line;

    public event Action<Vector2> lineUpdateEvent;
    public event Action LinkEvent;

    private void Update()
    {
        lineUpdateEvent?.Invoke(InputManager.MousePoint);
    }

    /// <summary>
    /// 마우스로 잡기 시작했을 때 호출 함수
    /// </summary>
    public void CreateLine()
    {
        line = GameManager.instance.lineObjectPool.GetObject();

        line.Link(transform);
        lineUpdateEvent += line.OnUpdate;
        line.gameObject.SetActive(true);
    }

    public void Reset()
    {
        lineUpdateEvent -= line.OnUpdate;
        line = null;
    }

    /// <summary>
    /// 만들어진 선 연결 함수
    /// </summary>
    public void ConnectLine(LinkedObject linkedObject)
    {
        if (this != linkedObject)
        {
            linkedObject.Set(line);
            Set(line);

            lineUpdateEvent?.Invoke(linkedObject.transform.position);
            LinkEvent?.Invoke();
        }
        else
        {
            DeleteLine();
        }

        Reset();
    }

    /// <summary>
    /// 만들어진 선 연결해제 함수
    /// </summary>
    public void UnConnectLine()
    {
        
    }

    /// <summary>
    /// 만들어진 선 연결해제 함수
    /// </summary>
    public void DeleteLine()
    {
        line.Unlink();
    }

    public void Set(Line line)
    {
        _lineList.Add(line);
    }
}
