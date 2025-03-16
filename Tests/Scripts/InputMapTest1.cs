using System;
using Bakery.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Bakery.Inputs.Test
{

    [CreateAssetMenu(fileName = "TestInputMap1", menuName = "Bakery/Test/TestInputMap1")]
    public class InputMapTest1 : InputMap, TestInputs.ITest1Actions
    {
        public static event Action OnTest1 = delegate { };
        private TestInputs.Test1Actions _inputMap;

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
            _inputMap = inputs.Test1;
            inputs.Test1.SetCallbacks(this);
        }




        public void OnTest1Action(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnTest1.Invoke();
        }

        public override void Shutdown()
        {
            _inputMap.Disable();
        }
    }

}