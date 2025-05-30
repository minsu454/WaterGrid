using Common.DotweenEx;
using Common.EnumExtensions;
using Common.Time;
using System;
using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private RectTransform clockwiseRectTr;
    [SerializeField] private float monthDuration;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private MonthType type = MonthType.Jan;

    private DotweenEx dotween;

    public void Init()
    {
        dayText.text = type.EnumToString();
        dotween = new DotweenEx(0, monthDuration, -360, () => { dotween = null; }).SetLoop().OnCompleted(OnCompleted);
    }

    private void Update()
    {
        dotween.OnUpdate();

        clockwiseRectTr.localEulerAngles =
            new Vector3(
            clockwiseRectTr.localEulerAngles.x,
            clockwiseRectTr.localEulerAngles.y,
            dotween.Value);
    }

    private void OnCompleted()
    {
        SetDay();
        InGameLoader.Map.UpgradeMap();
    }

    private void SetDay()
    {
        type = (MonthType)((int)(type + 1) % 12);
        dayText.text = type.EnumToString();
    }

    public void OnPause()
    {
        TimeType.InGame.SetTime(0f);
    }

    public void OnPlay()
    {
        TimeType.InGame.SetTime(1f);
    }

    public void On2xSpeed()
    {
        TimeType.InGame.SetTime(2f);
    }
}