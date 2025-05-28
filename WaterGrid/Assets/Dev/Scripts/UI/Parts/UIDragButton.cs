using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIDragButton : UIButton
{
    protected event Action UpdateEvent;
    [SerializeField] protected UIDrag dragUI;
    [SerializeField] protected Sprite sprite;

    /// <summary>
    /// 버튼을 눌렀을 때
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        InputManager.Instance.isUIPress = true;

        UpdateEvent += OnUpdate;
        dragUI.Show(sprite);
    }

    /// <summary>
    /// 버튼을 땠을 때
    /// </summary>
    public override void OnPointerUp(PointerEventData eventData)
    {
        dragUI.Hide();
        UpdateEvent -= OnUpdate;
        StartCoroutine(CoSetIsUIPress());
    }

    private void Update()
    {
        UpdateEvent?.Invoke();
    }

    protected IEnumerator CoSetIsUIPress()
    {
        yield return null;
        InputManager.Instance.isUIPress = false;
        OnCompleted();
    }

    protected void OnUpdate()
    {
        dragUI.OnUpdate();
    }

    protected abstract void OnCompleted();
}
