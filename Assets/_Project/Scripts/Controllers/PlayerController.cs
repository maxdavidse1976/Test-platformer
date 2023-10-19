using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonspiritGames.TestPlatformer
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
    public class PlayerController : InputController
    {
        PlayerInputActions _inputActions;
        bool _isJumping;

        void OnEnable()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Gameplay.Enable();
            _inputActions.Gameplay.Jump.started += JumpStarted;
            _inputActions.Gameplay.Jump.canceled += JumpCanceled;
        }

        void OnDisable()
        {
            _inputActions.Gameplay.Disable();
            _inputActions.Gameplay.Jump.started -= JumpStarted;
            _inputActions.Gameplay.Jump.canceled -= JumpCanceled;
            _inputActions = null;
        }

        void JumpCanceled(InputAction.CallbackContext obj)
        {
            _isJumping = false;
        }

        void JumpStarted(InputAction.CallbackContext obj)
        {
            _isJumping = true;
        }

        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return _isJumping;
        }

        public override float RetrieveMoveInput(GameObject gameObject)
        {
            return _inputActions.Gameplay.Move.ReadValue<Vector2>().x;
        }
    }
}