using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pump : NodeObject, IObjectPoolable<Pump>
{
    public event Action<Pump> ReturnEvent;

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
}
