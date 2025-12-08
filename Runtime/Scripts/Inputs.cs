using System;
using UnityEngine;

namespace Bakery
{
    public static class Inputs
    {
        public static class Events
        {
            public static Action OnPointerExit = delegate { };

            public static Action OnPointerEnter = delegate { };

            public static Action<InputMap> OnInputMapChanged = delegate { };
        }

        public static Func<IInputManager> Manager = UnregisterManager;

        internal static IInputManager UnregisterManager()
        {
            Debug.LogWarning("No InputManager registered, using MockInputManager");
            _cachedMockManager ??= new MockInputManager();
            Manager = () => _cachedMockManager;
            return _cachedMockManager;
        }

        public static IInputManager _cachedMockManager;

        public class MockInputManager : IInputManager
        {
            public InputMap CurrentMap => null;
            public GameObject FirstObjectUnderCursor => null;
            public void RevertMap() { }
            public void SetMap(InputMap map) { }
        }
    }

}