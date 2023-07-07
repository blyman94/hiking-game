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

        private Hand _handLeft;
        private Hand _handRight;
        private Hand _currentActiveHand;
        private Hand _newActiveHand;


        #region MonoBehaviour Methods
        private void Awake()
        {
            _handLeft = Instantiate(_handLeftPrefab).GetComponent<Hand>();
            _handRight = Instantiate(_handRightPrefab).GetComponent<Hand>();
            HideHands();
        }
        #endregion

        public void OnHandPlaced()
        {
            if (_currentActiveHand == _handLeft)
            {
                _handLeft.Place();
            }
            else if (_currentActiveHand == _handRight)
            {
                _handRight.Place();
            }
            _currentActiveHand = null;
        }

        public void OnHoverStay(Vector3 targetPosition, Vector3 targetNormal)
        {
            SetActiveHand(targetPosition);
            if (_currentActiveHand != null)
            {
                _currentActiveHand.Move(targetPosition);
                _currentActiveHand.Rotate(transform,targetPosition);
            }
        }
        public void OnHoverEnd()
        {
            if (_currentActiveHand == null)
            {
                return;
            }
            if ((_currentActiveHand == _handLeft && !_handLeft.Placed) ||
                _currentActiveHand == _handRight && !_handRight.Placed)
            {
                _currentActiveHand.Hide();
            }
            _currentActiveHand = null;
        }

        private void MoveHand(HandType hand, Vector3 targetPosition)
        {
            if (hand == HandType.Left)
            {
                _handLeft.Move(targetPosition);
            }
            else if (hand == HandType.Right)
            {
                _handRight.Move(targetPosition);
            }
        }

        private void HideHands()
        {
            _handLeft.Hide();
            _handRight.Hide();
        }

        private void SetActiveHand(Vector3 targetPosition)
        {
            if (_handLeft.Placed && _handRight.Placed)
            {
                return;
            }
            if (_playerOrientation.IsToLeft(targetPosition))
            {
                if (_handLeft.Placed)
                {
                    _handRight.Show();
                    _newActiveHand = _handRight;
                }
                else
                {
                    _handLeft.Show();
                    _newActiveHand = _handLeft;
                }
            }
            else
            {
                if (_handRight.Placed)
                {
                    _handLeft.Show();
                    _newActiveHand = _handLeft;
                }
                else
                {
                    _handRight.Show();
                    _newActiveHand = _handRight;
                }
            }

            if (_newActiveHand != _currentActiveHand)
            {
                if (_currentActiveHand != null)
                {
                    _currentActiveHand.Hide();
                }
                _currentActiveHand = _newActiveHand;
                _currentActiveHand.Show();
            }
        }
    }
}

