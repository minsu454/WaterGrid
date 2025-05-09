using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public sealed class InputManager : MonoBehaviour
{
    [SerializeField] private InputType inputType;       //인풋 타입
    private LinkedObject firstLinkedObj;

    public static Vector2 MousePoint
    {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    }

    /// <summary>
    /// 클릭 시 호출 함수
    /// </summary>
    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && EventSystem.current.IsPointerOverGameObject() is false)
        {
            Performed();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Canceled();
        }
    }

    /// <summary>
    /// 키다운 시 호출 함수
    /// </summary>
    public void Performed()
    {
        if (GetSelected(out LinkedObject link) is false)
            return;

        link.CreateLine();
        firstLinkedObj = link;
    }

    /// <summary>
    /// 키 땠을 시 호출 함수
    /// </summary>
    public void Canceled()
    {
        if (GetSelected(out LinkedObject link) is true)
        {
            firstLinkedObj.ConnectLine(link);
        }
        else
        {
            firstLinkedObj.DeleteLine();
            firstLinkedObj.Reset();
        }

        firstLinkedObj = null;
    }

    /// <summary>
    /// 레이쏴서 블록에 맞은 것을 저장해주는 함수
    /// </summary>
    private bool GetSelected(out LinkedObject link)
    {
        link = null;

        if (IsMouseHit(out RaycastHit2D hit) is false)
            return false;

        if (hit.collider.TryGetComponent(out link) is false)
            return false;

        return true;
    }

    /// <summary>
    /// 마우스 클릭방향으로 레이를 쏴서 spawnpoint가 있는지 체크하는 함수 
    /// </summary>
    public bool IsMouseHit(out RaycastHit2D hit)
    {
        Vector2 vec = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        hit = Physics2D.Linecast(vec, vec * 5);

        if (!hit)
            return false;

        return true;
    }
}
