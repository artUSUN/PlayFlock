using UnityEngine;

namespace PlayFlock.GameLogic
{
    public interface ISpawnable
    {
        bool TryPlace(Vector3 coordinates);
    }
}
