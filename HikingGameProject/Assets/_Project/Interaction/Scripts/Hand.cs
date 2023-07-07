using Blyman94.CommonSolutions;
using UnityEngine;

namespace HikingGame.Interaction
{
    public class Hand : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _maxDistanceFromChest = 2.0f;

        [Header("Component References")]
        [SerializeField] private Transform _graphics;
        [SerializeField] private Animator _animator;

        [Header("Scriptable Object References")]
        [SerializeField] private Vector3Variable _playerChestPosition;

        public bool Placed { get; set; }

        private int _handCloseLoopHash;
        private int _handOneThirdClosedIdleHash;
        private Vector3 _graphicsOffset;
        private float _maxDistanceFromChestSqr;

        private bool _shown;

        #region MonoBehaviour Methods
        private void Awake()
        {
            _graphicsOffset = _graphics.localPosition;

            _maxDistanceFromChestSqr = Mathf.Pow(_maxDistanceFromChest, 2);

            _handCloseLoopHash = Animator.StringToHash("Hand_Close_Loop");
            _handOneThirdClosedIdleHash = Animator.StringToHash("Hand_OneThirdClosed_Idle");
        }
        private void Update()
        {
            if (_shown)
            {
                if ((transform.position - _playerChestPosition.Value).sqrMagnitude > 
                    _maxDistanceFromChestSqr)
                {
                    Reset();
                }
            }
        }
        #endregion

        public void Move(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        public void Place()
        {
            _graphics.localPosition =
                new Vector3(_graphicsOffset.x, 0.0f, _graphicsOffset.z);
            _animator.Play(_handOneThirdClosedIdleHash);
            Placed = true;
        }

        public void Hide()
        {
            _graphics.gameObject.SetActive(false);
            _shown = false;
        }

        public void Show()
        {
            _graphics.gameObject.SetActive(true);
            _shown = true;
        }

        public void Rotate(Transform origin, Vector3 targetPosition)
        {
            var lookPos = origin.position - transform.position;
            var rotation = Quaternion.LookRotation(-lookPos);
            transform.rotation = rotation;
        }

        public void Reset()
        {
            _graphics.localPosition = _graphicsOffset;
            _animator.Play(_handCloseLoopHash);
            Placed = false;
            Hide();
        }
    }
}
