using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class GroupElement
    {
        public Transform Transform { get; private set; }
        public IGroupable GroupableScript { get; private set; }
        public Transform PrimordialRoot { get; private set; }

        public GroupElement(Transform transform, IGroupable groupable, Transform parent)
        {
            Transform = transform;
            GroupableScript = groupable;
            PrimordialRoot = parent;
        }
    }
}
