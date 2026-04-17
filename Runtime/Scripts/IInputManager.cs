using UnityEngine;

namespace Bakery
{
    public interface IInputManager
    {
        Vector2 CursorPosition { get; set; }
        InputMap CurrentMap { get; }
        GameObject FirstObjectUnderCursor { get; }
        void SetExclusiveMap(InputMap map);
        void AddMap(InputMap map);
        void RemoveMap(InputMap map);
        void RevertMap();

    }
}

