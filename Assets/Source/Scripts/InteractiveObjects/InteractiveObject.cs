using PlayFlock.MainGameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock.InteractiveObjects
{
    [RequireComponent(typeof(Outline))]
    public abstract class InteractiveObject : MonoBehaviour, ISpawnable, IDestroyable, IRelocateable, IBindable, ISelectable
    {
        protected Outline outline;
        protected string bindingPointName = "Binding Point";
        public Dictionary<IBindable, Transform> Bindings { get; protected set; }

        protected virtual void Awake()
        {
            Bindings = new Dictionary<IBindable, Transform>();
            outline = GetComponent<Outline>();
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
            point.transform.SetParent(transform, false);
            Bindings.Add(partner, point.transform);
            bindingPoint = point.transform;
        }

        public virtual void RemoveBind(IBindable partner)
        {
            var point = Bindings[partner];
            Bindings.Remove(partner);
            Destroy(point.gameObject);
        }

        public void DrawOutline()
        {
            outline.enabled = true;
        }

        public void EraseOutline()
        {
            outline.enabled = false;
        }
    }
}
