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

    public override void OnDisconnectLineParent(int cost)
    {
        
    }

    public override void OnDisconnectLineChildren(int cost)
    {
        parentNodeObject = null;
    }

    protected override void SetText()
    {
        costText.text = $"{MyCost}";
    }

    
}
