using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bakery.Inputs
{

    public class InputManager : MonoBehaviour, CoreInputs.ICoreActions
    {
        [SerializeField] private List<InputMap> _maps = new();

        [SerializeField] private InputMap _defaultMap;

        [SerializeField] private LayerMask _interactableLayer;

        private CoreInputs _coreInputs;

        //private Inputs _inputs;

        private InputMap _previousMap;
        private InputMap _currentMap;
        private Vector2 _cursorPosition;
        private Camera _camera;

        private GameObject _firstObjectUnderCursor;

        void Awake()
        {
            _camera = Camera.main;
            _coreInputs = new CoreInputs();
            _coreInputs.Core.SetCallbacks(this);
            // _inputs = new Inputs();
        }

        void OnEnable()
        {
            InputServices.SwitchInputMap = SetInputMap;
            InputServices.RevertInputMap = RevertInputMap;
            InputServices.GetFirstObjectUnderCursor = () => _firstObjectUnderCursor;
            InputServices.GetCurrentInputMap = () => _currentMap;
            _coreInputs.Enable();
        }



        void OnDisable()
        {
            _coreInputs.Disable();
            InputServices.SwitchInputMap = delegate { };
            InputServices.RevertInputMap = delegate { };
            InputServices.GetFirstObjectUnderCursor = delegate { return null; };
            InputServices.GetCurrentInputMap = delegate { return null; };
        }

        void Start()
        {
            foreach (var map in _maps)
                map.Init();

            SetInputMap(_defaultMap);
        }

        void OnDestroy()
        {
            foreach (var map in _maps)
                map.Shutdown();
        }

        void FixedUpdate()
        {
            if (PointerIsOnUI()) return;
            if (PointerIsOnInteractable()) return;
        }

        private bool PointerIsOnInteractable()
        {
            Ray ray = _camera.ScreenPointToRay(_cursorPosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, _interactableLayer))
            {
                if (_firstObjectUnderCursor == null) return false;
                InputEvents.OnPointerExit?.Invoke();
                _firstObjectUnderCursor = null;
                return false;
            }
            // Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject == _firstObjectUnderCursor) return true;

            if (_firstObjectUnderCursor != null)
                InputEvents.OnPointerExit?.Invoke();

            _firstObjectUnderCursor = hit.collider.gameObject;
            InputEvents.OnPointerEnter?.Invoke();
            return true;
        }

        private bool PointerIsOnUI()
        {
            if (!RaycastUtilities.PointerIsOverUI(_cursorPosition, out GameObject hitObject))
                return false;
            //Debug.Log(hitObject.name);
            if (_firstObjectUnderCursor != null)
                InputEvents.OnPointerExit?.Invoke();


            if (hitObject != null)
            {
                _firstObjectUnderCursor = hitObject;
                InputEvents.OnPointerEnter?.Invoke();
            }
            else
                _firstObjectUnderCursor = null;

            return true;

        }

        private void RevertInputMap()
        {
            if (_previousMap == null)
            {
                Debug.LogWarning("Input Services: No previous input map to revert to");
                return;
            }
            SetInputMap(_previousMap);
        }

        private void SetInputMap(InputMap newMap)
        {
            if (_currentMap == newMap) return;
            foreach (var map in _maps)
            {
                map.IsEnabled = map == newMap;
            }
            _previousMap = _currentMap;
            _currentMap = newMap;
            InputEvents.OnInputMapChanged?.Invoke(newMap);
        }

        public void OnCursorPosition(InputAction.CallbackContext context)
        {
            _cursorPosition = context.ReadValue<Vector2>();
        }
    }
}
