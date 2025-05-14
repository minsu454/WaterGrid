using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public sealed class InputManager : MonoBehaviour
{
    [SerializeField] private InputType inputType;       //인풋 타입

    public static Vector2 inputWorldPoint
    {
        get { return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); }
    }

    private bool isPress;

    private void Update()
    {
        if (Managers.Node.TempInteractionable == null || isPress is false)
            return;

        Managers.Node.TempInteractionable.Pressed();
    }

    /// <summary>
    /// 클릭 시 호출 함수
    /// </summary>
    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && EventSystem.current.IsPointerOverGameObject() is false)
        {
            Performed();
            isPress = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isPress = false;
            Canceled();
        }
    }

    /// <summary>
    /// 키다운 시 호출 함수
    /// </summary>
    public void Performed()
    {
        if (Managers.Node.GetSelected(out Interactionable interaction) is false)
            return;

        interaction.Performed();
    }

    /// <summary>
    /// 키 땠을 시 호출 함수
    /// </summary>
    public void Canceled()
    {
        if (Managers.Node.GetSelected(out Interactionable interaction) is true)
        {
            interaction.Canceled();
        }
        else if (Managers.Node.TempInteractionable is Line)
        {
            Managers.Node.CancelLine((Line)Managers.Node.TempInteractionable);
        }
        else
        {
            Managers.Node.DeleteLine();
        }
    }
}
