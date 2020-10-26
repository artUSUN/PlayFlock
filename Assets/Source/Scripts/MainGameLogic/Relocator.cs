using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Relocator : MonoBehaviour
    {
        [SerializeField] private int iterations = 50;

        private const int ignoreRaycastLayer = 2; 
        private int selectedObjectLayer;
        private Transform relocateable;
        private IRelocateable relocateableScript;

        //Listen to OnClickedDown from Raycaster
        public void SelectTarget(RaycastHit hitInfo)
        {
            relocateableScript = hitInfo.transform.GetComponent<IRelocateable>();

            if (relocateableScript != null)
            {
                relocateable = hitInfo.transform;
                selectedObjectLayer = relocateable.gameObject.layer;
                relocateable.gameObject.layer = ignoreRaycastLayer;
            }
        }

        //Listen to OnDrag from Raycaster
        public void RelocateTarget(RaycastHit hitInfo)
        {
            if (relocateableScript != null)
            {
                Vector3 hitPosition = hitInfo.point;
                if (relocateableScript.TryPlace(hitPosition)) relocateable.position = hitPosition;
                else
                {
                    #region Trying to find the closest point to spawn
                    bool canSpawn;
                    Vector3 lastAvailableSpawnPos = relocateable.position;
                    Vector3 step = (hitPosition - relocateable.position) / iterations;
                    do
                    {
                        lastAvailableSpawnPos += step;
                        canSpawn = relocateableScript.TryPlace(lastAvailableSpawnPos);
                    } while (canSpawn);

                    relocateable.position = lastAvailableSpawnPos - step;
                    #endregion

                    //Trying to follow mouse pointer on each side
                    if (relocateable.position.x != hitPosition.x)
                    {
                        float lastAvailableSpawnPos_x = relocateable.position.x;
                        float stepX = (hitPosition.x - relocateable.position.x) / iterations;
                        for (int i = 0; i < iterations; i++)
                        {
                            lastAvailableSpawnPos_x += stepX;
                            canSpawn = relocateableScript.TryPlace(new Vector3(lastAvailableSpawnPos_x, relocateable.position.y, relocateable.position.z));
                            if (!canSpawn)
                            {
                                lastAvailableSpawnPos_x -= stepX;
                            }
                        }
                        relocateable.position = new Vector3(lastAvailableSpawnPos_x, relocateable.position.y, relocateable.position.z);
                    }
                    if (relocateable.position.z != hitPosition.z)
                    {
                        float lastAvailableSpawnPos_z = relocateable.position.z;
                        float stepZ = (hitPosition.z - relocateable.position.z) / iterations;

                        for (int i = 0; i < iterations; i++)
                        {
                            lastAvailableSpawnPos_z += stepZ;
                            canSpawn = relocateableScript.TryPlace(new Vector3(relocateable.position.x, relocateable.position.y, lastAvailableSpawnPos_z));
                            if (!canSpawn)
                            {
                                lastAvailableSpawnPos_z -= stepZ;
                            }
                        }
                        relocateable.position = new Vector3(relocateable.position.x, relocateable.position.y, lastAvailableSpawnPos_z);
                    }
                }
            }
        }

        //Listen to ClickedUp from Input
        public void UnselectTarget()
        {
            if (relocateableScript != null)
            {
                relocateable.gameObject.layer = selectedObjectLayer;
                relocateable = null;
                relocateableScript = null;
            }
        }
    }
}
