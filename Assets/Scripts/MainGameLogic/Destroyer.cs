using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private float maxDistanceBetweenClicks = 0.15f;

        private LayerMask whatIsInteractive;
        private RaycastHit firstClickHit;

        private void Start()
        {
            whatIsInteractive = LayerMask.GetMask("Interactive");
        }

        //Listen to OnOneClicked from Raycaster
        public void SetFirstClickPos(RaycastHit hitInfo)
        {
            if (1 << hitInfo.collider.gameObject.layer == whatIsInteractive.value) firstClickHit = hitInfo;
        }

        //Listen to DoubleClicked from Raycaster
        public void Destroy(RaycastHit hitInfo)
        {
            Vector3 delta = hitInfo.point - firstClickHit.point;
            if (delta.sqrMagnitude <= maxDistanceBetweenClicks) Destroy(firstClickHit.collider.gameObject);
        }
    }
}
