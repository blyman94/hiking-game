using Blyman94.CommonSolutions;
using HikingGame.Common;
using UnityEngine;

namespace HikingGame.Climbing
{
    public delegate void HandPlaced();

    public class ClimbingRock : MonoBehaviour, IInteractable
    {
        public HandPlaced HandPlaced;
        [SerializeField] private GameEvent _handPlacedGlobalEvent;
        public bool PlayerInClimbingSpot { get; set; }
        public int HandsPlacedCount { get; set; }

        #region MonoBehaviour Methods
        private void Start()
        {
            PlayerInClimbingSpot = false;
            Reset();
        }
        #endregion

        #region IInteractable Methods & Properties
        public bool IsHovering { get; set; }
        public Vector3 HoverPosition { get; set; }
        public Vector3 InteractPosition { get; set; }
        public void OnInteract()
        {
            if (PlayerInClimbingSpot)
            {
                HandsPlacedCount++;
                HandPlaced?.Invoke();
                _handPlacedGlobalEvent.Raise();
            }
        }
        #endregion

        public void Reset()
        {
            PlayerInClimbingSpot = false;
            HandsPlacedCount = 0;
        }
    }
}
