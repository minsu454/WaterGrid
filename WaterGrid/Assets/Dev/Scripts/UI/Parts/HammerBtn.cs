using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HammerBtn : UIDragButton
{
    [SerializeField] private int upgradeCount;

    public override void Init()
    {
        base.Init();
        SetUseText();
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
        if (MapManager.Line.GetSelected(out Interactionable interaction) is false)
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