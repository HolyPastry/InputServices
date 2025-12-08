using UnityEngine;

namespace Bakery
{
    public interface IInputManager
    {
        InputMap CurrentMap { get; }
        GameObject FirstObjectUnderCursor { get; }
        void SetMap(InputMap map);
        void RevertMap();

    }
}

