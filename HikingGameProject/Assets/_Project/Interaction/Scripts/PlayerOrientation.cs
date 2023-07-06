using UnityEngine;

namespace HikingGame.Interaction
{
    public class PlayerOrientation : MonoBehaviour
    {
        [SerializeField] private Rigidbody _playerRigidbody;

        private bool _rotationLocked = false;
        private Transform _cameraTransform;

        #region MonoBehaviour Methods
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }
        private void FixedUpdate()
        {
            if (_playerRigidbody.velocity.sqrMagnitude > 0.01f)
            {
                _rotationLocked = false;
            }
            else
            {
                _rotationLocked = true;
            }
        }

        private void Update()
        {
            if (_rotationLocked)
            {
                return;
            }

            transform.rotation =
                Quaternion.Euler(transform.eulerAngles.x, _cameraTransform.eulerAngles.y,
                transform.eulerAngles.z);
        }
        #endregion

        public bool IsToLeft(Vector3 comparePoint)
        {
            Vector3 delta = (comparePoint - transform.position).normalized;
            Vector3 cross = Vector3.Cross(delta, transform.forward);

            if (cross.y >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
