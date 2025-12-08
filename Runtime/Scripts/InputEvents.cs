using System;
using Bakery.Inputs;

public static partial class InputEvents
{
    public static Action OnPointerExit = delegate { };

    public static Action OnPointerEnter = delegate { };

    public static Action<InputMap> OnInputMapChanged = delegate { };

}

