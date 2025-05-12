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

    public override void OnUnConnectLine(int cost)
    {
        curConnectCost = curConnectCost - cost;

        SetText();
    }

    protected override void SetText()
    {
        if (isNeedText is false)
            return;

        costText.text = $"{CurConnectCost} / {MaxConnectCost}";
    }
}
