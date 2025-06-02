using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Common.Event.Args.EventArgs;

public class HammerBtn : UIDragButton
{
    private int upgradeCount;

    public override void Init()
    {
        base.Init();
        count = InGameLoader.Item.Hammer;
        upgradeCount = InGameLoader.Dev.HammerUpgradeCount;

        OnActiveEvent();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (count == 0)
            return;

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (count == 0)
            return;

        base.OnPointerUp(eventData);
    }

    protected override void OnCompleted()
    {
        if (InGameLoader.Map.Line.GetSelected(out Interactionable interaction) is false)
            return;

        if (interaction is Pump)
        {
            Pump pump = (Pump)interaction;

            pump.Upgrade(upgradeCount);
            count--;
            OnActiveEvent();
            SetUseText();
        }
    }
}