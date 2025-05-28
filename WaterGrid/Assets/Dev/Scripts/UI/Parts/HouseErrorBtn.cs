using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HouseErrorBtn : UIButton
{
    public override void Init()
    {
        
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
