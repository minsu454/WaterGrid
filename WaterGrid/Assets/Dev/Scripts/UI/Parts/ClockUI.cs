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
        clockwiseRectTr.localEulerAngles = new Vector3(clockwiseRectTr.localEulerAngles.x, clockwiseRectTr.localEulerAngles.y, clockwiseRectTr.localEulerAngles.z - speed * Time.deltaTime);
    }
}