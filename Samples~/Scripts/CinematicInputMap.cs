using System;
using Bakery.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "CinematicInputMap", menuName = "Bakery/CinematicInputMap")]
public class CinematicInputMap : InputMap, Inputs.ICinematicActions
{
    public static event Action OnSkip = delegate { };
    private Inputs.CinematicActions _inputMap;

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
        _inputMap = inputs.Cinematic;
        inputs.Cinematic.SetCallbacks(this);
    }


    public void OnSkipAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkip.Invoke();
    }


    public override void Shutdown()
    {
        _inputMap.Disable();
    }
}

