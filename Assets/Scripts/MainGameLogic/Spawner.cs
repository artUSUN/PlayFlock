using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject spawnablePrefab; //Quick plug. If necessary, I can easily implement the change of this prefab and then get a full-fledged tool
        [SerializeField] private Transform containerForSpawnedObjects;
        [SerializeField] private LayerMask spawnPlaneLayer;
        private ISpawnable spawnableScript;

        private void Start()
        {
            spawnableScript = spawnablePrefab.GetComponent<ISpawnable>();
        }

        //Listen to OnOneClicked from Raycaster
        public void Spawn(RaycastHit hitInfo)
        {
            if (1 << hitInfo.transform.gameObject.layer == spawnPlaneLayer.value)
            {
                if (spawnableScript.TryPlace(hitInfo.point)) Instantiate(spawnablePrefab, hitInfo.point, Quaternion.identity, containerForSpawnedObjects);
            }
        }
    }
}
