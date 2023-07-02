using UnityEngine;

namespace HikingGame.Common
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private bool _lockCursorOnStart = true;

        #region MonoBehaviour Methods
        private void Start()
        {
            if (_lockCursorOnStart)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
        #endregion

        public void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
