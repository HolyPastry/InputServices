
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bakery.Inputs
{
    internal class RaycastUtilities
    {
        public static bool PointerIsOverUI(Vector2 screenPos, out GameObject hitObject)
        {
            if (screenPos == Vector2.zero)
            {
                hitObject = null;
                return false;
            }
            var hitObjects = UIRaycast(ScreenPosToPointerData(screenPos));
            if (hitObjects.Count == 0)
            {
                hitObject = null;
                return false;
            }
            hitObject = hitObjects[0];

            return hitObject.layer == LayerMask.NameToLayer("UI");
        }

        public static List<GameObject> UIRaycast(Vector2 screenPos)
            => UIRaycast(ScreenPosToPointerData(screenPos));

        //     public List<GameObject> UIRaycast(Vector2 screenPosition)
        // /     => UIRaycast(InputServices.GetMousePosition());

        private static List<GameObject> UIRaycast(PointerEventData pointerData)
        {
            var results = new List<RaycastResult>();
            if (EventSystem.current == null) Debug.LogError("Add an EventSystem to the scene");
            EventSystem.current.RaycastAll(pointerData, results);

            return results.ConvertAll(result => result.gameObject);
        }

        // public static Vector2 GetMouseUIPosition()
        //     => ScreenPosToPointerData(InputServices.GetMousePosition()).position;

        private static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
            => new(EventSystem.current) { position = screenPos };

        // internal static bool IsObjectUnderPointer(GameObject gameObject, Vector2 screenPos)
        // {

        //     List<GameObject> objList = UIRaycast(screenPos);
        //     if (objList.Count == 0) return false;

        //     foreach (var o in objList)
        //         if (o.CompareTag(gameObject.tag) && ReferenceEquals(o, gameObject))
        //             return true;

        //     return false;
        // }



        // internal static void GetHitPosition(BagItemComponent itemBeingPlaced, out Vector3 worldPosition)
        // {
        //     var pointerData = ScreenPosToPointerData(InputServices.GetMousePosition());
        //     var results = new List<RaycastResult>();
        //     EventSystem.current.RaycastAll(pointerData, results);

        //     if (results.Count == 0)
        //     {
        //         worldPosition = InputServices.GetMousePosition();
        //         return;
        //     }
        //     worldPosition = results[0].worldPosition;
        // }
    }
}
