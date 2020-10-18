using UnityEngine;

namespace PlayFlock.GameLogic
{
    public interface IRelocateable
    {
        bool TryPlace(Vector3 coordinates);
    }
}
