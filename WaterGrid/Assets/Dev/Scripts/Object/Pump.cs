using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pump : NodeObject
{
    public override bool IsConnectCost(NodeObject linkedObject)
    {
        int temp = curConnectCost + linkedObject.MyCost;

        if (curConnectCost > MaxConnectCost)
            return true;

        return false;
    }

    public override void OnConnectLineParent(NodeObject parent, Line line)
    {
        int temp = curConnectCost + parent.MyCost;

        if (curConnectCost > MaxConnectCost)
            return;

        curConnectCost = temp;

        //AddLine(linkObj, line);
        SetText();
    }

    public override void OnConnectLineChildren(NodeObject children, Line line)
    {

    }

    protected override void OnUnConnectLine(int cost)
    {

    }

    protected override void SetText()
    {
        if (isNeedText is false)
            return;

        costText.text = $"{CurConnectCost} / {MaxConnectCost}";
    }
}
