using UnityEngine;

namespace PlayFlock
{
    public interface ISpawnable
    {
        bool TrySpawn(Vector3 coordinates);
    }
}
