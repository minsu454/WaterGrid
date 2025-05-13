using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class NodeObject : MonoBehaviour, Interactionable
{
    [Header("MyObject")]
    [SerializeField] private NodeObjectType myType;             //내 타입
    public NodeObjectType Type
    {
        get { return myType; }
    }
    [SerializeField] private int myCost;                        //내 비용
    public int MyCost
    {
        get { return myCost; }
    }

    [SerializeField] private bool isTopObject = false;          //최상위 객체인지 확인 값
    private bool isConnectTopObject = false;                    //최상위 객체와 연결되어 있는지 확인 값
    public bool IsConnectTopObject
    {
        get { return isConnectTopObject; }
    }

    protected NodeObject parentNodeObject = null;               //부모 객체 오브젝트
    public NodeObject ParentNodeObject
    {
        get { return parentNodeObject; }
    }

    [Header("OtherObject")]
    [SerializeField] private NodeObjectType connectType;      //연결 가능한 타입
    public NodeObjectType ConnectType
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

    [Header("Text")]
    [SerializeField] protected bool isNeedText;                 //텍스트 필요한지 확인 값
    [SerializeField] protected TextMeshPro costText;            //코스트 텍스트

    private void Awake()
    {
        Initialized();
    }

    public void Initialized()
    {
        SetText();
    }

    /// <summary>
    /// 해당 오브젝트와 연결이 가능한지 알려주는 함수
    /// </summary>
    public bool CanConnect(NodeObject linkedObject)
    {
        return linkedObject.ConnectType.HasFlag(myType) && connectType.HasFlag(linkedObject.Type) && IsConnectCost(linkedObject) && linkedObject.IsConnectCost(this);
    }

    public abstract bool IsConnectCost(NodeObject linkedObject);

    /// <summary>
    /// 연결 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnConnectLineParent(NodeObject parent);

    /// <summary>
    /// 연결 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnConnectLineChildren(int childrenCost);

    /// <summary>
    /// 연결 해제 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnUnConnectLine(int cost);

    /// <summary>
    /// 텍스트 설정 함수
    /// </summary>
    protected abstract void SetText();

    public void Performed()
    {
        Managers.Node.CreateLine(this);
    }

    public void Canceled()
    {
        Managers.Node.TryAdd(this);
    }
}
