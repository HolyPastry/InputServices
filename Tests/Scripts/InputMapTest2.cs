using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Bakery.Inputs.Test
{
    [CreateAssetMenu(fileName = "TestInputMap2", menuName = "Bakery/Test/TestInputMap2")]
    public class InputMapTest2 : InputMap, TestInputs.ITest2Actions
    {

        public static event Action<Vector2> OnTest2 = delegate { };

        private TestInputs.Test2Actions _inputMap;

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
            TestInputs inputs = new();
            inputs.Enable();
            _inputMap = inputs.Test2;
            inputs.Test2.SetCallbacks(this);

        }


        public void OnTest2Action(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnTest2.Invoke(context.ReadValue<Vector2>());
        }

        public override void Shutdown()
        {
            _inputMap.Disable();
        }
    }

}