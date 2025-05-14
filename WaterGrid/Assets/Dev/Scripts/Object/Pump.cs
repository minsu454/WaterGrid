using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pump : NodeObject
{
    public override bool IsConnectCost(NodeObject linkedObject)
    {
        int temp = curConnectCost + linkedObject.MyCost;

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

    public override void OnUnConnectLineParent(int cost)
    {
        curConnectCost = curConnectCost - cost;

        SetText();
    }

    public override void OnUnConnectLineChildren(int cost)
    {
        curConnectCost = curConnectCost - cost;

        parentNodeObject = null;
        SetText();
    }

    protected override void SetText()
    {
        costText.text = $"{CurConnectCost} / {MaxConnectCost}";
    }
}
