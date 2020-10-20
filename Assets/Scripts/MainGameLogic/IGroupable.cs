using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public interface IGroupable
    {
        bool IsInGroup();
        void AddInGroup(Group group);
        void RemoveFromGroup();
        Group GetGroup();
    }
}
