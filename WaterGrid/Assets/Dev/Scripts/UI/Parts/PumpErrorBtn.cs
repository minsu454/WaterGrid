using UnityEngine.EventSystems;

public class PumpErrorBtn : UIButton
{
    public override void Init()
    {
        base.Init();

        OnActiveEvent();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnCompleted();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {

    }

    protected void OnCompleted()
    {
        
    }
}