using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class LinkedObject : MonoBehaviour
{
    [Header("MyObject")]
    [SerializeField] private LinkedObjectType myType;           //내 타입
    public LinkedObjectType Type
    {
        get { return myType; }
    }
    [SerializeField] private int myCost;                        //내 비용

    [Header("OtherObject")]
    [SerializeField] private LinkedObjectType connectType;      //연결 가능한 타입
    public LinkedObjectType ConnectType
    {
        get { return connectType; }
    }

    private int curConnectCost = 0;                             //현재 연결 비용
    [SerializeField] private int maxConnectCost;                //최대 연결 비용

    private List<Line> _lineList = new List<Line>();            //연결된 라인 
    private Line line;

    public event Action<int, Vector2> lineUpdateEvent;               //라인위치 업데이트 이벤트

    [Header("Text")]
    [SerializeField] private bool isNeedText;                   //텍스트 필요한지 확인 값
    [SerializeField] private TextMeshPro costText;              //코스트 텍스트

    public void Reset()
    {
        lineUpdateEvent -= line.OnUpdate;
        line = null;
    }

    private void Update()
    {
        lineUpdateEvent?.Invoke(1, InputManager.MousePoint);
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

    /// <summary>
    /// 만들어진 선 연결 함수
    /// </summary>
    public void ConnectLine(LinkedObject linkedObject)
    {
        if (this != linkedObject && linkedObject.ConnectType.HasFlag(myType) && connectType.HasFlag(linkedObject.Type))
        {
            linkedObject.AddLine(line);
            AddLine(line);

            lineUpdateEvent?.Invoke(1, linkedObject.transform.position);
            OnConnectLine();
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
    public void DeleteLine()
    {
        line.Unlink();
    }

    /// <summary>
    /// 라인을 오브젝트에 추가해주는 함수
    /// </summary>
    private void AddLine(Line line)
    {
        _lineList.Add(line);
    }

    /// <summary>
    /// 연결 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnConnectLine();

    /// <summary>
    /// 연결 해제 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnUnConnectLine();
}
