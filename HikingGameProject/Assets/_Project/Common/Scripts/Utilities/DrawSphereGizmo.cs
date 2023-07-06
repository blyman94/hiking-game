using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HikingGame.Common
{
    public class DrawSphereGizmo : MonoBehaviour
    {
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private float _radius;

        #region MonoBehaviour Methods
        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        #endregion
    }
}