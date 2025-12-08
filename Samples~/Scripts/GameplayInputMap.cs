using System;
using Bakery.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;



[CreateAssetMenu(fileName = "GameplayInputMap", menuName = "Bakery/GameplayInputMap")]
public class GameplayInputMap : InputMap, Inputs.IGameplayActions
{
    public static event Action OnPrimaryAction = delegate { };
    public static event Action<Vector2> OnMove = delegate { };
    public static event Action<Vector2> OnCursorDelta = delegate { };
    public static event Action OnCancel = delegate { };

    private Inputs.GameplayActions _inputMap;

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
        _inputMap = inputs.Gameplay;
        inputs.Gameplay.SetCallbacks(this);
    }

    public void OnCancelAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnCancel.Invoke();
    }

    public void OnCursorDeltaAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnCursorDelta.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnMove.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryActionAction(InputAction.CallbackContext context)
    {
        if (context.performed) OnPrimaryAction.Invoke();
    }


    public override void Shutdown()
    {
        _inputMap.Disable();
    }
}

