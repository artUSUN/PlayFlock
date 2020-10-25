using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private float maxDistanceBetweenClicks = 0.15f;
        private RaycastHit firstClickHit;

        //Listen to OnOneClicked from Raycaster
        public void SetFirstClickPos(RaycastHit hitInfo)
        {
            firstClickHit = hitInfo;
        }

        //Listen to DoubleClicked from Raycaster
        public void Destroy(RaycastHit hitInfo)
        {
            Vector3 delta = hitInfo.point - firstClickHit.point;
            if (delta.sqrMagnitude <= maxDistanceBetweenClicks)
            {
                IDestroyable destroyable = firstClickHit.collider.GetComponent<IDestroyable>();
                if (destroyable != null)
                {
                    destroyable.Destroy();
                }
            }
        }
    }
}
