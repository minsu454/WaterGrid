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
    private const float moveTimer = 1.5f;
    private bool isMyTr = true;
    private bool isConnectMouse = false;
    private bool isFirstIn = false;

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

    public override void Performed()
    {
        base.Performed();

        curTime = 0;
        isMyTr = true;
        isFirstIn = false;
        isConnectMouse = false;
    }

    public override void Pressed()
    {
        base.Pressed();

        if (isMyTr is false || ((MapManager.Line.GetSelected(out Interactionable interaction) is false || interaction != (Interactionable)this) && isConnectMouse is false))
        {
            isMyTr = false;
            return;
        }

        curTime += Time.deltaTime;

        if (moveTimer > curTime)
            return;
        else if (moveTimer <= curTime && isFirstIn is false)
        {
            MapManager.Line.DisconnectLine(parentNodeObject, this);
            MapManager.Line.DeleteAllLine(this);
            MapManager.Line.OtherWorkConnect();
            isConnectMouse = true;
            isFirstIn = true;
        }

        transform.position = InputManager.InputWorldPoint;
    }
}
