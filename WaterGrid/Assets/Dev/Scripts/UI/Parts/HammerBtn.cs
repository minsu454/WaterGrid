using UnityEngine;
using UnityEngine.EventSystems;

public class HammerBtn : ButtonUI
{
    [SerializeField] private int upgradeCount;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        
        base.OnPointerUp(eventData);
    }

    protected override void OnCompleted()
    {
        if (Managers.Node.GetSelected(out Interactionable interaction) is false)
            return;

        if (interaction is Pump)
        {
            Pump pump = (Pump)interaction;

            pump.Upgrade(upgradeCount);
        }
    }

    protected override void OnUpdate()
    {
        
    }
}