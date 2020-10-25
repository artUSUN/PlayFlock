using UnityEngine;

namespace PlayFlock.TwoPointLines
{
    [RequireComponent(typeof(LineRenderer))]
    public abstract class TwoPointLine : MonoBehaviour
    {
        protected LineRenderer line;

        protected virtual void Awake()
        {
            line = GetComponent<LineRenderer>();
        }

        public virtual void SetFirstPos(Vector3 pos)
        {
            line.SetPosition(0, pos);
        }
        
        public virtual void SetSecondPos(Vector3 pos)
        {
            line.SetPosition(1, pos);
        }
    }
}
