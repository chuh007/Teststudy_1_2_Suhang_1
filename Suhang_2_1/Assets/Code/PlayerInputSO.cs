using System;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsTarget;

        public event Action<bool> OnMouseStatusChange;

        public Vector2 MovementKey {get; private set;}
        public Vector2 MousePosition => _mousePosition;
        
        
        private Controls _control;
        private Vector2 _mousePosition;
        private Vector3 _worldPosition;

        private void OnEnable()
        {
            if (_control == null)
            {
                _control = new Controls();
                _control.Player.SetCallbacks(this);
            }
            _control.Player.Enable();
        }

        private void OnDisable()
        {
            _control.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementKey = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _mousePosition = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnMouseStatusChange?.Invoke(true);
            if(context.canceled)
                OnMouseStatusChange?.Invoke(false);
        }

        public Vector3 GetWorldPosition(out RaycastHit hit)
        {
            Camera mainCam = Camera.main;
            Ray camRay = mainCam.ScreenPointToRay(_mousePosition);
            bool isHit = Physics.Raycast(camRay,out hit, mainCam.farClipPlane, whatIsTarget);
            if (isHit)
            {
                _worldPosition = hit.point;
            }
            return _worldPosition;
        }
    }
}