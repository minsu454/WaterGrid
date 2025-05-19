using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected event Action UpdateEvent;

    /// <summary>
    /// 버튼을 눌렀을 때
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        InputManager.Instance.isUIPress = true;

        UpdateEvent += OnUpdate;
    }

    /// <summary>
    /// 버튼을 땠을 때
    /// </summary>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
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

    protected abstract void OnUpdate();

    protected abstract void OnCompleted();
}
