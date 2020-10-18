using UnityEngine;

namespace PlayFlock
{
    public interface ISpawnable
    {
        bool TrySpawn(Vector3 coordinates);
        void Spawn(Vector3 coordinates);
    }
}
