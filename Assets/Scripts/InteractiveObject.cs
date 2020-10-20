using PlayFlock.MainGameLogic;
using UnityEngine;

namespace PlayFlock
{
    public abstract class InteractiveObject : MonoBehaviour, ISpawnable, IDestroyable, IRelocateable, IGroupable
    {
        private bool isInGroup = false;
        private Group group;

        public abstract bool TryPlace(Vector3 coordinates);

        public virtual void Destroy() { Destroy(gameObject); }

        public virtual bool IsInGroup() { return isInGroup; }

        public virtual Group GetGroup() { return group; }

        public virtual void AddInGroup(Group group)
        {
            this.group = group;
            isInGroup = true;
        }

        public virtual void RemoveFromGroup()
        {
            this.group = null;
            isInGroup = false;
        }
    }
}
