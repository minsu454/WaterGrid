using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PumpBtn : UIDragButton
{
    public override void Init()
    {
        base.Init();
        SetUseText();
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
        MapManager.Instance.AddNode(InputManager.InputWorldPoint, TileType.Pump);
        count--;
        OnActiveEvent();
        SetUseText();
    }
}
