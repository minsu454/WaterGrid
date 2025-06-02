using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HouseErrorBtn : UIButton
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
        Transform errorTr = InGameLoader.Warning.WarningTransform(nameof(House));
        CameraManager.Instance.MoveCamera(errorTr);
    }
}
