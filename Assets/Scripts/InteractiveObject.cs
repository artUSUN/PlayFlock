using PlayFlock.MainGameLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayFlock
{
    public abstract class InteractiveObject : MonoBehaviour, ISpawnable, IDestroyable, IRelocateable, IBindable
    {
        protected string bindingPointName = "Binding Point";
        public Dictionary<IBindable, Transform> Bindings { get; protected set; }

        protected virtual void Start()
        {
            Bindings = new Dictionary<IBindable, Transform>();
        }

        public abstract bool TryPlace(Vector3 coordinates);

        public virtual void Destroy() { Destroy(gameObject); }

        public virtual bool IsInBindWith(IBindable target)
        {
            return Bindings.ContainsKey(target);
        }

        public virtual void AddBind(IBindable partner, out Transform bindingPoint)
        {
            var point = new GameObject(bindingPointName);
            point.transform.SetParent(transform);
            Bindings.Add(partner, point.transform);
            bindingPoint = point.transform;
        }

        public virtual void RemoveBind(IBindable partner)
        {
            var point = Bindings[partner];
            Bindings.Remove(partner);
            Destroy(point.gameObject);
        }
    }
}
