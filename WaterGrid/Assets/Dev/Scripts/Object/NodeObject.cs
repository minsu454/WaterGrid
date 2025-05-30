using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class NodeObject : MonoBehaviour, Interactionable
{
    [Header("MyObject")]
    [SerializeField] private TileType myType;               //내 타입
    public TileType Type
    {
        get { return myType; }
    }
    [SerializeField] protected int myCost;                  //내 비용
    public int MyCost
    {
        get { return myCost; }
    }

    [SerializeField] private bool isTopObject = false;          //최상위 객체인지 확인 값
    [SerializeField] private bool isConnectTopObject = false;   //최상위 객체와 연결되어 있는지 확인 값
    public bool IsConnectTopObject
    {
        get { return isConnectTopObject; }
    }

    protected NodeObject parentNodeObject = null;               //부모 객체 오브젝트
    public NodeObject ParentNodeObject
    {
        get { return parentNodeObject; }
    }

    protected int curConnectCost = 0;                           //현재 연결 비용
    public int CurConnectCost
    {
        get { return curConnectCost; }
    }

    [SerializeField] protected int maxConnectCost;              //최대 연결 비용
    public int MaxConnectCost
    {
        get { return maxConnectCost; }
    }

    [SerializeField] private Transform AreaTr;                  //범위
    [SerializeField] protected int radius;                      //반지름 
    public int Radius
    {
        get { return radius; }
    }

    [Header("OtherObject")]
    [SerializeField] private TileType connectType;              //연결 가능한 타입
    public TileType ConnectType
    {
        get { return connectType; }
    }

    [Header("Text")]
    [SerializeField] protected TextMeshPro costText;            //코스트 텍스트

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if(AreaTr != null)
            AreaTr.localScale = new Vector2(radius, radius);

        SetIsConnectTopObject(isTopObject);
        SetText();
    }

    #region Line

    /// <summary>
    /// 해당 오브젝트와 연결이 가능한지 알려주는 함수
    /// </summary>
    public bool CanConnect(NodeObject linkedObject)
    {
        return linkedObject.ConnectType.HasFlag(myType) && connectType.HasFlag(linkedObject.Type) && IsConnectCost(linkedObject.MyCost) && linkedObject.IsConnectCost(MyCost);
    }

    /// <summary>
    /// 최상위 오브젝트와 연결 되어있는지 확인 값 설정 함수
    /// </summary>
    public virtual void SetIsConnectTopObject(bool value)
    {
        isConnectTopObject = value;
    }

    /// <summary>
    /// 연결 시 필요한 요구 코스트가 충족되는지 반환 함수
    /// </summary>
    public abstract bool IsConnectCost(int cost);

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
    public abstract void OnDisconnectLineParent(int cost);

    /// <summary>
    /// 연결 해제 시 호출 이벤트 함수
    /// </summary>
    public abstract void OnDisconnectLineChildren(int cost);

    #endregion

    /// <summary>
    /// 업그레이드 함수
    /// </summary>
    public abstract void Upgrade(int count);

    /// <summary>
    /// 텍스트 설정 함수
    /// </summary>
    protected abstract void SetText();

    #region Input

    public virtual void Performed()
    {
        InGameLoader.Map.Line.CreateTempLine(this);
    }

    public virtual void Pressed() { }

    public virtual void Canceled()
    {
        InGameLoader.Map.Line.TryAdd(this);
    }

    #endregion
}
