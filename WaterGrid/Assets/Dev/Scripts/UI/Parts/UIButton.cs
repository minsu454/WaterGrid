using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header(nameof(UIButton))]
    [SerializeField] protected int count;
    [SerializeField] protected TextMeshProUGUI text;

    public abstract void Init();

    public abstract void OnPointerDown(PointerEventData eventData);

    public abstract void OnPointerUp(PointerEventData eventData);

    protected void SetUseText()
    {
        text.text = count.ToString();
    }
}