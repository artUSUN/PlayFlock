using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public interface IRelocateable
    {
        bool TryPlace(Vector3 coordinates);
    }
}
