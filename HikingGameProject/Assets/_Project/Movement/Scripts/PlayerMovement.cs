using UnityEngine;

namespace HikingGame.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private Rigidbody _rb;

        [Header("Movement Parameters")]
        [SerializeField] private float _maxSpeed = 2.0f;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float _decelRate = 0.1f;

        private Transform _cameraTransform;

        public Vector2 MoveInput { get; set; }

        #region MonoBehaviour Methods
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            Move();
        }
        #endregion

        private void Move()
        {
            Vector3 movementForwardDir = new Vector3(_cameraTransform.forward.x,
                0.0f, _cameraTransform.forward.z).normalized;
            Vector3 movementRightDir = new Vector3(_cameraTransform.right.x, 0.0f,
                _cameraTransform.right.z).normalized;

            Vector3 movementNormalized = (movementForwardDir * MoveInput.y +
                movementRightDir * MoveInput.x).normalized;

            if (MoveInput != Vector2.zero)
            {
                _rb.AddForce(movementNormalized * _maxSpeed, ForceMode.VelocityChange);
                _rb.AddForce(Vector3.right * -_rb.velocity.x, ForceMode.VelocityChange);
                _rb.AddForce(Vector3.forward * -_rb.velocity.z, ForceMode.VelocityChange);
            }
            else
            {
                _rb.AddForce(Vector3.right * (_decelRate * -_rb.velocity.x), ForceMode.VelocityChange);
                _rb.AddForce(Vector3.forward * (_decelRate * -_rb.velocity.z), ForceMode.VelocityChange);
            }
        }
    }
}
