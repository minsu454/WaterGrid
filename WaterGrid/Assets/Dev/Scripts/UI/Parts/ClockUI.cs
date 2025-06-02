using Common.DotweenEx;
using Common.EnumExtensions;
using Common.Time;
using System;
using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private RectTransform clockwiseRectTr;
    private float monthDuration;
    [SerializeField] private TextMeshProUGUI dayText;
    private MonthType type = MonthType.Jan;

    private DotweenEx dotween;

    public void Init()
    {
        type = InGameLoader.Dev.MonthType;
        monthDuration = InGameLoader.Dev.monthDuration;

        dayText.text = type.EnumToString();
        dotween = new DotweenEx(0, monthDuration, -360, TimeType.InGame, () => dotween = null).SetLoop().OnCompleted(OnCompleted);
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

    /// <summary>
    /// 한달이 지났을 때 실행 함수
    /// </summary>
    private void OnCompleted()
    {
        SetDay();
        if((int)type % 2 == 0)
            InGameLoader.Map.UpgradeMap();
    }

    /// <summary>
    /// 날짜 변경 및 아이템 팝업띄워주는 함수
    /// </summary>
    private void SetDay()
    {
        type = (MonthType)((int)(type + 1) % 12);
        dayText.text = type.EnumToString();

        if (type == MonthType.Jan)
        {
            Managers.UI.CreatePopup<ItemPopup>();
        }
    }

    /// <summary>
    /// 퍼즈 함수
    /// </summary>
    public void OnPause()
    {
        TimeType.InGame.SetTime(0f);
    }

    /// <summary>
    /// 플레이 함수
    /// </summary>
    public void OnPlay()
    {
        TimeType.InGame.SetTime(1f);
    }

    /// <summary>
    /// 2배속 함수
    /// </summary>
    public void On2xSpeed()
    {
        TimeType.InGame.SetTime(2f);
    }
}