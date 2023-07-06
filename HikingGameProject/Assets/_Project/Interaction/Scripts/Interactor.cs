using HikingGame.Common;
using UnityEngine;

namespace HikingGame.Interaction
{
    public class Interactor : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private HandHandler _handHandler;

        [Header("Interaction Parameters")]
        [SerializeField] private float _interactDistance = 1.0f;
        [SerializeField] private LayerMask _detectLayers;

        private RaycastHit _hitInfo;
        private IInteractable _currentInteractable;
        private bool _isHovering = false;

        #region MonoBehaviour Methods
        private void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward,
                out _hitInfo, _interactDistance, _detectLayers))
            {
                _currentInteractable = _hitInfo.collider.GetComponent<IInteractable>();
                if (_currentInteractable != null && !_isHovering)
                {
                    _isHovering = true;
                }
            }
            else
            {
                if (_isHovering)
                {
                    _isHovering = false;
                    _handHandler.OnHoverEnd();
                }
                _currentInteractable = null;
            }

            if (_isHovering)
            {
                _handHandler.OnHoverStay(_hitInfo.point, _hitInfo.normal);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position +
                (transform.forward * _interactDistance));
        }
        #endregion

        public void Activate()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.InteractPosition = _hitInfo.point;
                _currentInteractable.OnInteract();
            }
        }
    }
}
