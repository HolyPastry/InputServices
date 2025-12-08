using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bakery
{

    public abstract class InputMap : ScriptableObject
    {

        public abstract void Init();
        public abstract void Shutdown();
        public abstract bool IsEnabled { get; set; }

    }


}
