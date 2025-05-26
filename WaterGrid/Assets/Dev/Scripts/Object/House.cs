using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : NodeObject, IWarningable, IObjectPoolable<House>
{

    [Header("House")]
    [SerializeField] private SpriteOutline outline;
    public SpriteOutline Outline { get { return outline; } }
    
    private WarningIcon warningIcon;
    public WarningIcon Icon { get { return warningIcon; } }

    public event Action<House> ReturnEvent;

    public override bool IsConnectCost(NodeObject linkedObject)
    {
        return true;
    }

    public override void SetIsConnectTopObject(bool value)
    {
        base.SetIsConnectTopObject(value);

        if (value is false)
        {
            if (warningIcon != null)
                return;

            warningIcon = GameManager.instance.warningIconObjectPool.GetObject();
            warningIcon.gameObject.SetActive(true);
            warningIcon.Init(this);
        }
        else
        {
            if (warningIcon == null)
                return;

            warningIcon.Stop();
            warningIcon = null;
        }
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

    public void SetOutLineAlpha(float value)
    {
        outline.color.a = value;
    }

    private void OnDisable()
    {
        ReturnEvent.Invoke(this);
    }
}