using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected event Action UpdateEvent;
    [SerializeField] protected DragUI dragUI;
    [SerializeField] protected Sprite sprite;

/// <summary>
/// 버튼을 눌렀을 때
/// </summary>
public virtual void OnPointerDown(PointerEventData eventData)
    {
        InputManager.Instance.isUIPress = true;

        UpdateEvent += OnUpdate;
        dragUI.Show(sprite);
    }

    /// <summary>
    /// 버튼을 땠을 때
    /// </summary>
    public virtual void OnPointerUp(PointerEventData eventData)
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
