using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pump : LinkedObject
{
    public override void OnConnectLine(int cost)
    {
        curConnectCost += cost;
        SetText();
    }

    public override void OnUnConnectLine(int cost)
    {

    }

    public override void SetText()
    {
        if (isNeedText is false)
            return;

        costText.text = $"{CurConnectCost} / {MaxConnectCost}";
    }
}
