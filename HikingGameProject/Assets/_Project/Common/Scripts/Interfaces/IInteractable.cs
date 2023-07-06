using UnityEngine;

namespace HikingGame.Common
{
    public interface IInteractable
    {
        Vector3 InteractPosition { get; set; }
        void OnInteract();
    }
}
