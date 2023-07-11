using HikingGame.Common;
using Blyman94.CommonSolutions;
using UnityEngine;

namespace HikingGame.Climbing
{
    public class ClimbingSpot : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private Transform _climbingDestination;
        [SerializeField] private ClimbingCameraSequence _climbingCameraSequence;

        [Header("Global Events")]
        [SerializeField] private GameEvent _resetPlayerAimAxesEvent;
        [SerializeField] private GameEvent _climbSequenceCompleteGlobalEvent;

        private ClimbingRock[] _climbingRocks;
        private Vector3 _firstHandPosition;
        private Vector3 _secondHandPosition;
        private Transform _playerTransform;
        private Transform _playerOrientation;

        #region MonoBehaviour Methods
        private void Awake()
        {
            _climbingRocks = GetComponentsInChildren<ClimbingRock>();
        }
        private void OnEnable()
        {
            foreach (ClimbingRock climbingRock in _climbingRocks)
            {
                climbingRock.HandPlaced += OnHandPlaced;
            }
            _climbingCameraSequence.Complete += OnClimbingSequenceComplete;
        }
        private void OnDisable()
        {
            foreach (ClimbingRock climbingRock in _climbingRocks)
            {
                climbingRock.HandPlaced -= OnHandPlaced;
            }
            _climbingCameraSequence.Complete -= OnClimbingSequenceComplete;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (ClimbingRock climbingRock in _climbingRocks)
                {
                    climbingRock.PlayerInClimbingSpot = true;
                    _playerTransform = other.transform;
                    _playerOrientation = 
                        _playerTransform.GetComponentInChildren<PlayerOrientation>().transform;
                }
                _climbingCameraSequence.Reset();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (ClimbingRock climbingRock in _climbingRocks)
                {
                    climbingRock.Reset();
                    _playerTransform = null;
                }
            }
        }
        #endregion

        private void OnHandPlaced()
        {
            int handsPlacedCount = 0;
            foreach (ClimbingRock climbingRock in _climbingRocks)
            {
                handsPlacedCount += climbingRock.HandsPlacedCount;
            }
            if (handsPlacedCount == 1)
            {
                foreach (ClimbingRock climbingRock in _climbingRocks)
                {
                    if (climbingRock.HandsPlacedCount > 0)
                    {
                        _firstHandPosition = climbingRock.InteractPosition;
                        return;
                    }
                }
            }
            if (handsPlacedCount == 2)
            {
                foreach (ClimbingRock climbingRock in _climbingRocks)
                {
                    if (climbingRock.HandsPlacedCount > 0 && climbingRock.InteractPosition != _firstHandPosition)
                    {
                        _secondHandPosition = climbingRock.InteractPosition;
                    }
                }

                // Calculate new start point
                Vector3 firstHandRelativePos = _playerOrientation.InverseTransformPoint(_firstHandPosition);
                Vector3 secondHandRelativePos = _playerOrientation.InverseTransformPoint(_secondHandPosition);
                float xPos = (firstHandRelativePos.x + _secondHandPosition.x) * 0.5f;
                _climbingCameraSequence.SetDollyTrackStartXPosition(xPos);

                Vector3 lookForwardPos = (_firstHandPosition + _secondHandPosition) * 0.5f;
                _climbingCameraSequence.Run(_secondHandPosition, lookForwardPos, _firstHandPosition, _secondHandPosition);
            }
        }

        private void OnClimbingSequenceComplete()
        {
            _playerTransform.position = _climbingDestination.position;
            _resetPlayerAimAxesEvent.Raise();
            _climbSequenceCompleteGlobalEvent.Raise();
        }
    }
}
