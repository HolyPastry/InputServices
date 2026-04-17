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

            public Vector2 CursorPosition
            {
                get => Vector2.zero;
                set { }
            }

            public void AddMap(InputMap map)
            { }

            public void RemoveMap(InputMap map)
            {
            }

            public void RevertMap() { }
            public void SetExclusiveMap(InputMap map) { }
        }

        //Cleaning stuff in case cowboys are fast reloading in the editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Events.OnPointerExit = delegate { };
            Events.OnPointerEnter = delegate { };
            Events.OnInputMapChanged = delegate { };
            Manager = UnregisterManager;


#if UNITY_EDITOR
            Debug.Log("[Flow] Static fields reset (domain reload skipped)");
#endif
        }
    }

}