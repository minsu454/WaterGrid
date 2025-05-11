using System;
using System.Collections.Generic;
using TMPro;
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
    public int MyCost
    {
        get { return myCost; }
    }

    [Header("OtherObject")]
    [SerializeField] private LinkedObjectType connectType;      //연결 가능한 타입
    public LinkedObjectType ConnectType
    {
        get { return connectType; }
    }

    protected int curConnectCost = 0;                           //현재 연결 비용
    public int CurConnectCost
    {
        get { return curConnectCost; }
    }

    [SerializeField] private int maxConnectCost;                //최대 연결 비용
    public int MaxConnectCost
    {
        get { return maxConnectCost; }
    }

    public event Action<int, Vector2> lineUpdateEvent;          //라인위치 업데이트 이벤트

    [Header("Text")]
    [SerializeField] protected bool isNeedText;                 //텍스트 필요한지 확인 값
    [SerializeField] protected TextMeshPro costText;            //코스트 텍스트

    private Line tempLine;                                                                      //임시 라인
    private Dictionary<LinkedObject, Line> _lineDict = new Dictionary<LinkedObject, Line>();    //연결된 라인 

    private void Awake()
    {
        Initialized();
    }

    public void Initialized()
    {
        SetText();
    }

    public void Reset()
    {
        lineUpdateEvent -= tempLine.OnUpdate;
        tempLine = null;
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
        tempLine = GameManager.instance.lineObjectPool.GetObject();

        tempLine.Link(transform);
        lineUpdateEvent += tempLine.OnUpdate;
        tempLine.gameObject.SetActive(true);
    }

    /// <summary>
    /// 만들어진 선 연결 함수
    /// </summary>
    public void ConnectLine(LinkedObject linkedObject)
    {
        if (this != linkedObject && _lineDict.ContainsKey(linkedObject) is false && CanConnect(linkedObject))
        {
            linkedObject.AddLine(this, tempLine);
            AddLine(linkedObject, tempLine);

            lineUpdateEvent?.Invoke(1, linkedObject.transform.position);
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
        tempLine.Unlink();
    }

    /// <summary>
    /// 라인을 오브젝트에 추가해주는 함수
    /// </summary>
    private void AddLine(LinkedObject linkObj, Line line)
    {
        _lineDict.Add(linkObj, line);
        OnConnectLine(linkObj.myCost);
    }

    /// <summary>
    /// 해당 오브젝트와 연결이 가능한지 알려주는 함수
    /// </summary>
    private bool CanConnect(LinkedObject linkedObject)
    {
        return linkedObject.ConnectType.HasFlag(myType) && connectType.HasFlag(linkedObject.Type);
    }

    /// <summary>
    /// 연결 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnConnectLine(int cost);

    /// <summary>
    /// 연결 해제 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnUnConnectLine(int cost);

    /// <summary>
    /// 텍스트 설정 함수
    /// </summary>
    public abstract void SetText();
}
