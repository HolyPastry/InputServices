using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bakery
{

    public class InputManager : MonoBehaviour, IInputManager, CoreInputs.ICoreActions
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

        public InputMap CurrentMap => _currentMap;

        public GameObject FirstObjectUnderCursor => _firstObjectUnderCursor;

        void Awake()
        {
            _camera = Camera.main;
            _coreInputs = new CoreInputs();
            _coreInputs.Core.SetCallbacks(this);
            // _inputs = new Inputs();
        }

        void OnEnable()
        {

            _coreInputs.Enable();
            Inputs.Manager = () => this;
            foreach (var map in _maps)
                map.Init();

            SetExclusiveMap(_defaultMap);
        }



        void OnDisable()
        {
            _coreInputs.Disable();
            Inputs.Manager = Inputs.UnregisterManager;
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
                Inputs.Events.OnPointerExit?.Invoke();
                _firstObjectUnderCursor = null;
                return false;
            }
            // Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject == _firstObjectUnderCursor) return true;

            if (_firstObjectUnderCursor != null)
                Inputs.Events.OnPointerExit?.Invoke();

            _firstObjectUnderCursor = hit.collider.gameObject;
            Inputs.Events.OnPointerEnter?.Invoke();
            return true;
        }

        private bool PointerIsOnUI()
        {
            if (!RaycastUtilities.PointerIsOverUI(_cursorPosition, out GameObject hitObject))
                return false;
            //Debug.Log(hitObject.name);
            if (_firstObjectUnderCursor != null)
                Inputs.Events.OnPointerExit?.Invoke();


            if (hitObject != null)
            {
                _firstObjectUnderCursor = hitObject;
                Inputs.Events.OnPointerEnter?.Invoke();
            }
            else
                _firstObjectUnderCursor = null;

            return true;

        }

        public void RevertMap()
        {
            if (_previousMap == null)
            {
                Debug.LogWarning("Input Services: No previous input map to revert to");
                return;
            }
            SetExclusiveMap(_previousMap);
        }

        public void SetExclusiveMap(InputMap newMap)
        {
            if (_currentMap == newMap) return;
            foreach (var map in _maps)
            {
                map.IsEnabled = map == newMap;
            }
            _previousMap = _currentMap;
            _currentMap = newMap;
            Inputs.Events.OnInputMapChanged?.Invoke(newMap);
        }

        public void OnCursorPosition(InputAction.CallbackContext context)
        {
            _cursorPosition = context.ReadValue<Vector2>();
        }

        public void AddMap(InputMap map)
        {
            map.IsEnabled = true;
        }

        public void RemoveMap(InputMap map)
        {
            map.IsEnabled = false;
        }
    }
}
