using Common.DotweenEx;
using Common.Timer;
using System;
using UnityEngine;

public class House : NodeObject, IWarningable, IObjectPoolable<House>
{
    [Header("House")]
    [SerializeField] private SpriteOutline outline;
    public SpriteOutline Outline { get { return outline; } }
    
    private WarningIcon warningIcon;
    public WarningIcon Icon { get { return warningIcon; } }

    private UIButtonType errorType = UIButtonType.HouseErrorBtn;
    public UIButtonType ErrorType { get { return errorType; } }

    public event Action<House> ReturnEvent;

    private DotweenEx dotween;
    private float scoreDelayTime = 3f;

    public override void Init()
    {
        dotween = new DotweenEx(0, scoreDelayTime, 1, TimeType.InGame, () => dotween = null).SetLoop().OnCompleted(() => InGameLoader.Game.PlusScore(MyCost));
        base.Init();
    }

    public override bool IsConnectCost(int cost)
    {
        return true;
    }

    public override void SetIsConnectTopObject(bool value)
    {
        base.SetIsConnectTopObject(value);

        if (value is false)
        {
            if (warningIcon != null)
                return;

            warningIcon = InGameLoader.Warning.GetObject();
            warningIcon.gameObject.SetActive(true);
            warningIcon.Init(this);

            dotween.Stop();
        }
        else
        {
            if (warningIcon == null)
                return;

            warningIcon.Stop();
            warningIcon = null;

            dotween.ReStart();
        }
    }

    public override void OnConnectLineParent(NodeObject parent)
    {
        parentNodeObject = parent;
    }

    public override void OnConnectLineChildren(int childrenCost)
    {
        
    }

    public override void OnDisconnectLineParent(int cost)
    {
        
    }

    public override void OnDisconnectLineChildren(int cost)
    {
        parentNodeObject = null;
    }

    protected override void SetText()
    {
        costText.text = $"{MyCost}";
    }

    public override void Upgrade(int count)
    {
        if (parentNodeObject == null)
        {

        }
        else if (parentNodeObject.IsConnectCost(count) is true)
        {
            parentNodeObject.OnConnectLineChildren(count);
        }
        else
        {
            InGameLoader.Map.Line.DisconnectLine(parentNodeObject, this);
        }

        myCost += count;
        SetText();
    }

    public void SetOutLineAlpha(float value)
    {
        outline.color.a = value;
    }

    private void Update()
    {
        dotween.OnUpdate();
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }

    
}