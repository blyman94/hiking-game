using UnityEngine;

namespace HikingGame.Common
{
    /// <summary>
    /// Clears the attached transforms's parent on start.
    /// </summary>
    public class UnparentOnStart : MonoBehaviour
    {
        #region MonoBehaviour Methods
        private void Start()
        {
            transform.SetParent(null);
        }
        #endregion
    }
}

