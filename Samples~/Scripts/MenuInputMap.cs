using System;
using Bakery.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "MenuInputMap", menuName = "Bakery/MenuInputMap")]
public class MenuInputMap : InputMap, Inputs.IMenuActions
{
    public static event Action OnCancel = delegate { };
    public static event Action<Vector2> OnCursorDelta = delegate { };
    public static event Action<Vector2> OnCursorPosition = delegate { };
    public static event Action OnMain = delegate { };
    public static event Action<Vector2> OnScroll = delegate { };

    private Inputs.MenuActions _inputMap;

    public override bool IsEnabled
    {
        get => _inputMap.enabled;
        set
        {
            if (value) _inputMap.Enable();
            else _inputMap.Disable();
        }
    }

    public override void Init()
    {
        Inputs inputs = new();
        inputs.Enable();
        _inputMap = inputs.Menu;
        inputs.Menu.SetCallbacks(this);
    }

    public void OnCancelAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnCancel.Invoke();

    }

    public void OnCursorPositionAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnCursorPosition.Invoke(context.ReadValue<Vector2>());
    }

    public void OnCursorDeltaAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnCursorDelta.Invoke(context.ReadValue<Vector2>());
    }

    public void OnScrollAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnScroll.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMainAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnMain.Invoke();
    }

    public override void Shutdown()
    {
        throw new NotImplementedException();
    }
}

