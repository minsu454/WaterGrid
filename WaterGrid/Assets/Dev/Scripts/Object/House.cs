using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : LinkedObject
{
    public override void OnConnectLine(int cost)
    {
        SetText();
    }

    public override void OnUnConnectLine(int cost)
    {
        
    }

    public override void SetText()
    {
        if (isNeedText is false)
            return;

        costText.text = $"{MyCost}";
    }
}
