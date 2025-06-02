using UnityEngine.EventSystems;
using static Common.Event.Args.EventArgs;

public class PumpBtn : UIDragButton
{
    public override void Init()
    {
        base.Init();
        count = InGameLoader.Item.Pump;
        OnActiveEvent();
    }

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
        InGameLoader.Map.AddNode(InputManager.InputWorldPoint, TileType.Pump);
        count--;
        OnActiveEvent();
        SetUseText();
    }
}
