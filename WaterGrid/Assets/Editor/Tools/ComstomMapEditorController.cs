using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public sealed class ComstomMapEditorController
{
    public event Action<Vector3> leftMouseDownEvent;     //왼쪽 마우스 다운 이벤트
    public event Action<Vector3> leftMouseDragEvent;       //왼쪽 마우스 드래그 이벤트 
    public event Action<Vector3> leftMouseUpEvent;       //왼쪽 마우스 업 이벤트 

    public event Action<Vector3> rightMouseDownEvent;    //오른쪽 마우스 다운 이벤트
    public event Action<Vector3> rightMouseDragEvent;    //오른쪽 마우스 드래그 이벤트
    public event Action<Vector3> rightMouseUpEvent;      //오른쪽 마우스 업 이벤트

    /// <summary>
    /// Get Editor Event 함수
    /// </summary>
    public Event GetEvent()
    {
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(controlId);

        return Event.current;
    }

    /// <summary>
    /// 키 입력 함수
    /// </summary>
    public void InputMouse(Event e)
    {
        if (e.button == 0)
        {
            MouseEvent(e, leftMouseDownEvent, leftMouseDragEvent, leftMouseUpEvent);
        }
        else if (e.button == 1)
        {
            MouseEvent(e, rightMouseDownEvent, rightMouseDragEvent, rightMouseUpEvent);
        }
    }

    /// <summary>
    /// 마우스 입력 이벤트 함수
    /// </summary>
    private void MouseEvent(Event e, Action<Vector3> mouseDown, Action<Vector3> mouseDrag, Action<Vector3> mouseUp)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                mouseDown?.Invoke(e.mousePosition);
                e.Use();
                break;
            case EventType.MouseDrag:
                mouseDrag?.Invoke(e.mousePosition);
                e.Use();
                break;
            case EventType.MouseUp:
                mouseUp?.Invoke(e.mousePosition);
                e.Use();
                break;
        }

        InternalEditorUtility.RepaintAllViews();
    }
}