using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public interface ISpawnable
    {
        bool TryPlace(Vector3 coordinates);
    }
}
