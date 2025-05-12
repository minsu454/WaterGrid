using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : NodeObject
{
    public override bool IsConnectCost(NodeObject linkedObject)
    {
        int temp = curConnectCost + linkedObject.MyCost;

        if (curConnectCost > MaxConnectCost)
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
