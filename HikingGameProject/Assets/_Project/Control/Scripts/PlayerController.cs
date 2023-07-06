using HikingGame.Interaction;
using HikingGame.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HikingGame.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Interactor _playerInteractor;
        public void OnMoveInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerMovement.MoveInput = context.ReadValue<Vector2>();
            }
        }
        public void OnActivateInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _playerInteractor.Activate();
            }
        }
    }
}
