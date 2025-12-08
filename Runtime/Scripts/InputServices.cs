using System;
using Bakery.Inputs;
using UnityEngine;


public static partial class InputServices
{
    public static Action<InputMap> SwitchInputMap = delegate { };

    public static Action RevertInputMap = delegate { };

    public static Func<InputMap> GetCurrentInputMap = delegate { return null; };


    public static Func<GameObject> GetFirstObjectUnderCursor = delegate { return null; };
}

