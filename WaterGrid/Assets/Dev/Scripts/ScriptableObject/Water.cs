using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : NodeObject, IObjectPoolable<Water>
{
    public event Action<Water> ReturnEvent;

    public override bool IsConnectCost(int cost)
    {
        int temp = curConnectCost + cost;

        if (temp > MaxConnectCost)
            return false;

        return true;
    }

    public override void OnConnectLineParent(NodeObject parent)
    {
        
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
        
    }

    protected override void SetText()
    {
        costText.text = $"{CurConnectCost} / {MaxConnectCost}";
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }

    public override void Upgrade(int count)
    {
        
    }
}
