using Common.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Common.Event.Args.EventArgs;

public abstract class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header(nameof(UIButton))]
    [SerializeField] protected UIButtonType buttonType;
    [SerializeField] protected int count = 0;

    [SerializeField] protected TextMeshProUGUI text;

    public virtual void Init()
    {
        EventManager.Subscribe(GameEventType.ButtonEvent, OnActiveEvent);
    }

    /// <summary>
    /// gameObject 꺼졌을 때 켜주는 이벤트 함수
    /// </summary>
    private void OnActiveEvent(object args)
    {
        ButtonArgs buttonArgs = args as ButtonArgs;

        if (buttonArgs == null || buttonArgs.type != buttonType)
            return;

        count = buttonArgs.count;

        if (count == 0)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);

        SetUseText();
    }

    /// <summary>
    /// gameObject 꺼졌을 때 켜주는 이벤트 함수(내부용)
    /// </summary>
    protected void OnActiveEvent()
    {
        if (count == 0)
            gameObject.SetActive(false);

        SetUseText();
    }

    public abstract void OnPointerDown(PointerEventData eventData);

    public abstract void OnPointerUp(PointerEventData eventData);

    protected void SetUseText()
    {
        text.text = count.ToString();
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.ButtonEvent, OnActiveEvent);
    }
}