using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header(nameof(UIButton))]
    [SerializeField] private int count;
    public int Count
    {
        get { return count; }

        set
        {
            count = value;

            if (Count == 0)
                gameObject.SetActive(false);
        }
    }

    [SerializeField] protected TextMeshProUGUI text;

    public virtual void Init()
    {
        Count = count;
    }

    public abstract void OnPointerDown(PointerEventData eventData);

    public abstract void OnPointerUp(PointerEventData eventData);

    protected void SetUseText()
    {
        text.text = count.ToString();
    }
}