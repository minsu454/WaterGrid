using Common.Timer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    private Coroutine coScore;
    private float scoreDelayTime = 3f;

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

            warningIcon = WarningManager.Instance.warningIconObjectPool.GetObject();
            warningIcon.gameObject.SetActive(true);
            warningIcon.Init(this);

            if(coScore != null)
                StopCoroutine(coScore);
        }
        else
        {
            if (warningIcon == null)
                return;

            warningIcon.Stop();
            warningIcon = null;

            StartCoroutine(CoTimer.Loop(scoreDelayTime, () => GameManager.Instance.PlusScore(MyCost)));
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
            MapManager.Line.DisconnectLine(parentNodeObject, this);
        }

        myCost += count;
        SetText();
    }

    public void SetOutLineAlpha(float value)
    {
        outline.color.a = value;
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }

    
}