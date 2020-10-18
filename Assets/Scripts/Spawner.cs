using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject spawnablePrefab;
        [SerializeField] private Transform containerForSpawnedObj;
        [SerializeField] private LayerMask spawnPlaneLayer;

        private ISpawnable spawnableScript;
        private Camera cam;
        private const float rayLength = 100f;

        private void Start()
        {
            cam = Camera.main;
            spawnableScript = spawnablePrefab.GetComponent<ISpawnable>();
        }

        public void Spawn(Vector2 screenCoord)
        {
            Ray ray = cam.ScreenPointToRay(screenCoord);
            Physics.Raycast(ray, out RaycastHit hitInfo, rayLength);
            Debug.Log(1<<hitInfo.collider.gameObject.layer);
            Debug.Log(spawnPlaneLayer.value);
            if (1<<hitInfo.collider.gameObject.layer == spawnPlaneLayer.value)
            {
                Debug.Log("works");
                if (spawnableScript.TrySpawn(hitInfo.point)) Instantiate(spawnablePrefab, hitInfo.point, Quaternion.identity, containerForSpawnedObj);
            }
        }
    }
}
