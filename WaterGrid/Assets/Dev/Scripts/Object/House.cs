using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : NodeObject
{
    public override bool IsConnectCost(NodeObject linkedObject)
    {
        return true;
    }

    public override void OnConnectLineParent(NodeObject parent)
    {
        parentNodeObject = parent;
    }

    public override void OnConnectLineChildren(int childrenCost)
    {
        
    }

    public override void OnUnConnectLine(int cost)
    {
        
    }

    protected override void SetText()
    {
        if (isNeedText is false)
            return;

        costText.text = $"{MyCost}";
    }

    
}
