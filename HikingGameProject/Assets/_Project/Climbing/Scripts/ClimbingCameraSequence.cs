using Blyman94.CommonSolutions;
using Cinemachine;
using System.Collections;
using UnityEngine;

namespace HikingGame.Climbing
{
    public delegate void Complete();

    /// <summary>
    /// A camera dolly sequence that makes the player appear to be climbing over 
    /// a rock.
    /// </summary>
    public class ClimbingCameraSequence : MonoBehaviour
    {
        public Complete Complete;

        [Header("Component References")]
        [SerializeField] private CinemachineVirtualCamera _climbingCamera;
        [SerializeField] private CinemachineDollyCart _climbingDollyCart;
        [SerializeField] private CinemachineSmoothPath _climbingDollyTrack;
        [SerializeField] private Transform _lookTarget;

        [Header("Sequence Parameters")]
        [Tooltip("Should the player look at their hand positions " +
            "before climbing?")]
        [SerializeField] private bool _playLookSequence = true;

        [Tooltip("How quickly should the player looks at their hand " +
            "positions?")]
        [SerializeField] private float _lookSpeed;

        [Tooltip("How long should the player pause at each hand position?")]
        [SerializeField] private float _timeBetweenLooks = 0.5f;

        [Tooltip("Multiplier to scale the speed of the climbing curve.")]
        [SerializeField] private float _climbSpeedMultiplier = 1.0f;

        [Tooltip("How long to wait before the ascent starts?")]
        [SerializeField] private float _climbAnticipationTime = 0.5f;

        [Tooltip("Curve describing the camera's speed throughout the ascent.")]
        [SerializeField] private AnimationCurve _climbSpeedCurve;

        [Header("Events")]
        [SerializeField] private GameEvent _removePlayerControlEvent;
        [SerializeField] private GameEvent _restorePlayerControlEvent;

        // TODO: Determine if this is necessary.
        private CinemachineSmoothPath.Waypoint _originalDollyTrackStartPosition;

        #region MonoBehaviour Methods
        private void Awake()
        {
            _climbingDollyCart.m_PositionUnits =
                CinemachinePathBase.PositionUnits.Normalized;
            _climbingCamera.Priority = 0;
        }
        #endregion

        public void Reset()
        {
            _climbingDollyCart.m_Position = 0.0f;
            _climbingDollyCart.m_Speed = 0.0f;
            
            // TODO: Remove this if it's not necessary.
            ResetDollyTrackStartPosition();
        }

        public void Run(Vector3 lookTargetStartPos, Vector3 lookForwardPos,
            Vector3 lookPosA, Vector3 lookPosB)
        {
            StartCoroutine(ClimbingCameraRoutine(lookTargetStartPos,
                lookForwardPos, lookPosA, lookPosB));
        }

        public void SetDollyTrackStartXPosition(float xPos)
        {
            _originalDollyTrackStartPosition = _climbingDollyTrack.m_Waypoints[0];
            CinemachineSmoothPath.Waypoint newStartPosition =
                new CinemachineSmoothPath.Waypoint();

            newStartPosition.position = new Vector3(xPos, 
                _originalDollyTrackStartPosition.position.y,
                _originalDollyTrackStartPosition.position.z);
                
            _climbingDollyTrack.m_Waypoints[0] = newStartPosition;
        }

        private void ResetDollyTrackStartPosition()
        {
            // TODO: Remove this if it's not necessary.
            _climbingDollyTrack.m_Waypoints[0] = 
                _originalDollyTrackStartPosition;
        }

        private IEnumerator ClimbingCameraRoutine(Vector3 lookTargetStartPos,
            Vector3 lookForwardPos, Vector3 lookPosA, Vector3 lookPosB)
        {
            _removePlayerControlEvent.Raise();

            float step = _lookSpeed * Time.deltaTime;

            _lookTarget.position = lookTargetStartPos;
            _climbingCamera.LookAt = _lookTarget;
            _climbingCamera.Priority = 100;

            if (_playLookSequence)
            {
                while (Vector3.Distance(_lookTarget.position,
                    lookPosA) >= 0.001f)
                {
                    _lookTarget.transform.position =
                        Vector3.MoveTowards(_lookTarget.transform.position,
                        lookPosA, step);
                    yield return null;
                }
                yield return new WaitForSeconds(_timeBetweenLooks);

                while (Vector3.Distance(_lookTarget.position,
                lookPosB) >= 0.001f)
                {
                    _lookTarget.transform.position =
                        Vector3.MoveTowards(_lookTarget.transform.position,
                        lookPosB, step);
                    yield return null;
                }
                yield return new WaitForSeconds(_timeBetweenLooks);
            }

            while (Vector3.Distance(_lookTarget.position,
                    lookForwardPos) >= 0.001f)
            {
                _lookTarget.transform.position =
                    Vector3.MoveTowards(_lookTarget.transform.position,
                    lookForwardPos, step);
                yield return null;
            }

            yield return new WaitForSeconds(_climbAnticipationTime);

            _climbingCamera.LookAt = null;
            while (_climbingDollyCart.m_Position < 1.0f)
            {
                _climbingDollyCart.m_Speed =
                    _climbSpeedCurve.Evaluate(_climbingDollyCart.m_Position) * _climbSpeedMultiplier;
                yield return null;
            }

            _climbingCamera.Priority = 0;

            _restorePlayerControlEvent.Raise();

            Complete?.Invoke();
        }
    }
}
