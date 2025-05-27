using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PumpBtn : ButtonUI
{
    [SerializeField] private int useCount;
    [SerializeField] private TextMeshProUGUI text;

    public override void Init()
    {
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

    }

    private void SetUseText()
    {
        text.text = useCount.ToString();
    }
}
