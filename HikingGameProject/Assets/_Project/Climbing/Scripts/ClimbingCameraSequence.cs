using Cinemachine;
using System.Collections;
using UnityEngine;

namespace HikingGame.Climbing
{
    /// <summary>
    /// A camera dolly sequence that makes the player appear to be climbing over 
    /// a rock.
    /// </summary>
    public class ClimbingCameraSequence : MonoBehaviour
    {
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

        #region MonoBehaviour Methods
        private void Awake()
        {
            _climbingDollyCart.m_PositionUnits =
                CinemachinePathBase.PositionUnits.Normalized;
        }
        #endregion

        public void Run(Vector3 lookTargetStartPos, Vector3 lookForwardPos, 
            Vector3 lookLeftPos, Vector3 lookRightPos)
        {
            StartCoroutine(ClimbingCameraRoutine(lookTargetStartPos, 
                lookForwardPos, lookLeftPos, lookRightPos));
        }

        private IEnumerator ClimbingCameraRoutine(Vector3 lookTargetStartPos, 
            Vector3 lookForwardPos, Vector3 lookLeftPos, Vector3 lookRightPos)
        {
            _lookTarget.position = lookTargetStartPos;
            _climbingCamera.LookAt = _lookTarget;

            if (_playLookSequence)
            {
                float step = _lookSpeed * Time.deltaTime;
                while (Vector3.Distance(_lookTarget.position,
                lookRightPos) >= 0.001f)
                {
                    _lookTarget.transform.position =
                        Vector3.MoveTowards(_lookTarget.transform.position,
                        lookRightPos, step);
                    yield return null;
                }
                yield return new WaitForSeconds(_timeBetweenLooks);

                while (Vector3.Distance(_lookTarget.position,
                    lookLeftPos) >= 0.001f)
                {
                    _lookTarget.transform.position =
                        Vector3.MoveTowards(_lookTarget.transform.position,
                        lookLeftPos, step);
                    yield return null;
                }
                yield return new WaitForSeconds(_timeBetweenLooks);

                while (Vector3.Distance(_lookTarget.position,
                    lookForwardPos) >= 0.001f)
                {
                    _lookTarget.transform.position =
                        Vector3.MoveTowards(_lookTarget.transform.position,
                        lookForwardPos, step);
                    yield return null;
                }
            }

            yield return new WaitForSeconds(_climbAnticipationTime);

            _climbingCamera.LookAt = null;
            while (_climbingDollyCart.m_Position < 1.0f)
            {
                _climbingDollyCart.m_Speed =
                    _climbSpeedCurve.Evaluate(_climbingDollyCart.m_Position) * _climbSpeedMultiplier;
                yield return null;
            }
        }
    }
}
