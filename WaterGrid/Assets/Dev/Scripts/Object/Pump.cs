using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pump : NodeObject, IObjectPoolable<Pump>, IWarningable
{
    private UIButtonType errorType = UIButtonType.PumpErrorBtn;
    public UIButtonType ErrorType { get; }

    public WarningIcon Icon { get; }

    public SpriteOutline Outline { get; }

    public event Action<Pump> ReturnEvent;

    private float curTime = 0;
    private const float moveTimer = 2f;
    private bool isMyTr = true;
    private bool isConnectMouse = false;

    public override bool IsConnectCost(int cost)
    {
        int temp = curConnectCost + cost;

        if (temp > MaxConnectCost)
            return false;

        return true;
    }

    public override void OnConnectLineParent(NodeObject parent)
    {
        curConnectCost = curConnectCost + parent.MyCost;
        parentNodeObject = parent;

        SetText();
    }

    public override void OnConnectLineChildren(int childrenCost)
    {
        curConnectCost = curConnectCost + childrenCost;

        SetText();
    }

    public override void OnDisconnectLineParent(int cost)
    {
        curConnectCost = curConnectCost - cost;

        SetText();
    }

    public override void OnDisconnectLineChildren(int cost)
    {
        curConnectCost = curConnectCost - cost;

        parentNodeObject = null;
        SetText();
    }

    protected override void SetText()
    {
        costText.text = $"{CurConnectCost} / {MaxConnectCost}";
    }

    public override void Upgrade(int count)
    {
        maxConnectCost += count;
        SetText();
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }

    public void SetOutLineAlpha(float value)
    {
        
    }

    public override void Pressed()
    {
        base.Pressed();

        if (isMyTr is false || ((MapManager.Line.GetSelected(out Interactionable interaction) is false || interaction != (Interactionable)this) && isConnectMouse is false))
        {
            isMyTr = false;
            return;
        }

        Debug.Log("in");

        curTime += Time.deltaTime;

        if (moveTimer <= curTime)
        {
            transform.position = InputManager.InputWorldPoint;
            MapManager.Line.OtherWorkConnect();
            isConnectMouse = true;
        }
    }

    public override void Canceled()
    {
        base.Canceled();
        curTime = 0;
        isMyTr = true;
        isConnectMouse = false; 
    }
}
