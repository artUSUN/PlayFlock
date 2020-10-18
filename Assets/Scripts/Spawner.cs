using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private LayerMask spawnPlaneLayer;

        public ISpawnable SpawnableObject { get; set; }

        private Camera cam;
        private const float rayLength = 100f;

        private void Start()
        {
            cam = Camera.main;
        }

        public void Spawn(Vector2 screenCoord)
        {
            Ray ray = cam.ScreenPointToRay(screenCoord);
            Physics.Raycast(ray, out RaycastHit hitInfo, rayLength);
            if (hitInfo.collider.gameObject.layer == spawnPlaneLayer)
            {
                if (SpawnableObject.TrySpawn(hitInfo.point)) Spawn(hitInfo.point);
            }
        }
    }
}
