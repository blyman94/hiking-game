using UnityEngine;

namespace HikingGame.Interaction
{
    public enum HandType { Left, Right }

    public class HandHandler : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private PlayerOrientation _playerOrientation;

        [Header("Prefab References")]
        [SerializeField] private GameObject _handLeftPrefab;
        [SerializeField] private GameObject _handRightPrefab;

        private GameObject _handLeft;
        private GameObject _handRight;
        private GameObject _currentActiveHand;
        private GameObject _newActiveHand;

        private bool _handLeftPlaced = false;
        private bool _handRightPlaced = false;

        #region MonoBehaviour Methods
        private void Awake()
        {
            _handLeft = Instantiate(_handLeftPrefab);
            _handRight = Instantiate(_handRightPrefab);
            HideHands();
        }
        #endregion

        public void Reset()
        {
            _handLeftPlaced = false;
            _handRightPlaced = false;
            _currentActiveHand = null;
            HideHands();
        }

        public void OnHandPlaced()
        {
            if (_currentActiveHand == _handLeft)
            {
                _handLeftPlaced = true;
            }
            else if (_currentActiveHand == _handRight)
            {
                _handRightPlaced = true;
            }
            _currentActiveHand = null;
        }

        public void OnHoverStay(Vector3 targetPosition, Vector3 targetNormal)
        {
            SetActiveHand(targetPosition);
            if (_currentActiveHand != null)
            {
                _currentActiveHand.transform.position = targetPosition;
                RotateHand(targetPosition, targetNormal);
            }
        }
        public void OnHoverEnd()
        {
            if (_currentActiveHand == null)
            {
                return;
            }
            if ((_currentActiveHand == _handLeft && !_handLeftPlaced) ||
                _currentActiveHand == _handRight && !_handRightPlaced)
            {
                _currentActiveHand.SetActive(false);
            }
            _currentActiveHand = null;
        }

        private void MoveHand(HandType hand, Vector3 targetPosition)
        {
            if (hand == HandType.Left)
            {
                _handLeft.transform.position = targetPosition;
            }
            else if (hand == HandType.Right)
            {
                _handRight.transform.position = targetPosition;
            }
        }

        private void RotateHand(Vector3 targetPosition, Vector3 targetNormal)
        {
            var lookPos = transform.position - _currentActiveHand.transform.position;
            var rotation = Quaternion.LookRotation(-lookPos);
            _currentActiveHand.transform.rotation = rotation;
        }

        private void HideHands()
        {
            _handLeft.SetActive(false);
            _handRight.SetActive(false);
        }

        private void SetActiveHand(Vector3 targetPosition)
        {
            if (_handLeftPlaced && _handRightPlaced)
            {
                return;
            }
            if (_playerOrientation.IsToLeft(targetPosition))
            {
                if (_handLeftPlaced)
                {
                    _handRight.SetActive(true);
                    _newActiveHand = _handRight;
                }
                else
                {
                    _handLeft.SetActive(true);
                    _newActiveHand = _handLeft;
                }
            }
            else
            {
                if (_handRightPlaced)
                {
                    _handLeft.SetActive(true);
                    _newActiveHand = _handLeft;
                }
                else
                {
                    _handRight.SetActive(true);
                    _newActiveHand = _handRight;
                }
            }

            if (_newActiveHand != _currentActiveHand)
            {
                if (_currentActiveHand != null)
                {
                    _currentActiveHand.SetActive(false);
                }
                _currentActiveHand = _newActiveHand;
                _currentActiveHand.SetActive(true);
            }
        }
    }
}

