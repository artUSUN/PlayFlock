using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public interface IBindable
    {
        bool IsInBindWith(IBindable target);

        void AddBind(IBindable partner, out Transform bindingPoint);
        void RemoveBind(IBindable partner);
    }
}
