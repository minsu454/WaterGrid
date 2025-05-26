using Common.DotweenEx;
using Common.Time;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private RectTransform clockwiseRectTr;
    [SerializeField] private float duration;

    private DotweenEx dotween;


    public void Init()
    {
        dotween = new DotweenEx(0, duration, -360, () => { dotween = null; }).SetLoop().OnCompleted(OnCompleted);
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
        Debug.Log("하루가 지났당");
    }

    public void OnPause()
    {
        TimeType.InGame.SetTime(0f);
    }

    public void OnPlay()
    {
        TimeType.InGame.SetTime(1f);
    }
}