using Common.Time;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private RectTransform clockwiseRectTr;
    [SerializeField] private float speed;

    public void Init()
    {
        
    }

    private void Update()
    {
        clockwiseRectTr.localEulerAngles =
            new Vector3(
            clockwiseRectTr.localEulerAngles.x,
            clockwiseRectTr.localEulerAngles.y,
            clockwiseRectTr.localEulerAngles.z - TimeType.InGame.Get() * speed * Time.deltaTime);
    }
}