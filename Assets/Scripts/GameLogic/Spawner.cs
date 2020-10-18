using UnityEngine;

namespace PlayFlock.GameLogic
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject spawnablePrefab; //Quick plug. If necessary, I can easily implement the change of this prefab and then get a full-fledged tool

        [SerializeField] private Transform containerForSpawnedObj;
        private LayerMask spawnPlaneLayer;

        private ISpawnable spawnableScript;

        private void Start()
        {
            spawnPlaneLayer = LayerMask.GetMask("Spawn Plane");
            spawnableScript = spawnablePrefab.GetComponent<ISpawnable>();
        }

        //Listen to OnOneClicked from Raycaster
        public void Spawn(RaycastHit hitInfo)
        {
            if (1 << hitInfo.collider.gameObject.layer == spawnPlaneLayer.value)
            {
                if (spawnableScript.TryPlace(hitInfo.point)) Instantiate(spawnablePrefab, hitInfo.point, Quaternion.identity, containerForSpawnedObj);
            }
        }
    }
}
