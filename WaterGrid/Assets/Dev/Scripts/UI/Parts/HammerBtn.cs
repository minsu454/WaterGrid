using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HammerBtn : ButtonUI
{
    [SerializeField] private int useCount;
    [SerializeField] private int upgradeCount;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (useCount == 0)
            return;

        base.OnPointerDown(eventData);

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (useCount == 0)
            return;

        base.OnPointerUp(eventData);
    }

    protected override void OnCompleted()
    {
        if (MapManager.Node.GetSelected(out Interactionable interaction) is false)
            return;

        if (interaction is Pump)
        {
            Pump pump = (Pump)interaction;

            pump.Upgrade(upgradeCount);
        }
    }
}