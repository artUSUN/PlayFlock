using SOEvents;
using System;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class OutlinePainter : MonoBehaviour
    {
        private ISelectable selectable;

        public void OnBindingStarted(Transform transform)
        {
            selectable = transform.GetComponent<ISelectable>();
            if (selectable != null)
            {
                selectable.DrawOutline();
            }
        }

        public void OnBindingFinished()
        {
            if (selectable != null)
            {
                selectable.EraseOutline();
                selectable = null;
            }
        }
    }
}
