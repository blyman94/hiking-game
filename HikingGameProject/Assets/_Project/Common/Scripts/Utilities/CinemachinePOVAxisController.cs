using Cinemachine;
using UnityEngine;

namespace HikingGame.Common
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachinePOVAxisController : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private CinemachinePOV _virtualPov;

        #region MonoBehaviour Methods
        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        private void Start()
        {
            _virtualPov =
                _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }
        #endregion

        public void ResetAimAxes()
        {
            SetAimAxes(0.0f, 0.0f);
        }

        public void SetAimAxes(float horizontal, float vertical)
        {
            _virtualPov.m_HorizontalAxis.Value = horizontal;
            _virtualPov.m_VerticalAxis.Value = vertical;
        }
    }
}
