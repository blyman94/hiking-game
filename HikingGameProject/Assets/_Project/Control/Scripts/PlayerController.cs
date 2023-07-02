using HikingGame.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HikingGame.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private PlayerMovement _playerMovement;
        public void OnMoveInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerMovement.MoveInput = context.ReadValue<Vector2>();
            }
        }
    }
}
