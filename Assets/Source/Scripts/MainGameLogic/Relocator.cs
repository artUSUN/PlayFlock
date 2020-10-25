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
        private bool isSelectedRelocateble = false, needRelocateTarget = false;
        private Vector3 lastHitPosition;


        //Listen to OnClickedDown from Raycaster
        public void SelectTarget(RaycastHit hitInfo)
        {
            relocateableScript = hitInfo.transform.GetComponent<IRelocateable>();

            if (relocateableScript != null)
            {
                relocateable = hitInfo.transform;
                isSelectedRelocateble = true;
                selectedObjectLayer = relocateable.gameObject.layer;
                relocateable.gameObject.layer = ignoreRaycastLayer;
            }
        }

        private void Update()
        {
            if (needRelocateTarget)
            {
                if (relocateableScript.TryPlace(lastHitPosition)) relocateable.position = lastHitPosition;
                else
                {
                    #region Trying to find the closest point to spawn
                    bool canSpawn;
                    Vector3 lastAvailableSpawnPos = relocateable.position;
                    Vector3 step = (lastHitPosition - relocateable.position)/iterations;
                    do
                    {
                        lastAvailableSpawnPos += step;
                        canSpawn = relocateableScript.TryPlace(lastAvailableSpawnPos);
                    } while (canSpawn);

                    relocateable.position = lastAvailableSpawnPos - step;
                    #endregion

                    //Trying to follow mouse pointer on each side
                    if (relocateable.position.x != lastHitPosition.x)
                    {
                        float lastAvailableSpawnPos_x = relocateable.position.x;
                        float stepX = (lastHitPosition.x - relocateable.position.x) / iterations;
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
                    if (relocateable.position.z != lastHitPosition.z)
                    {
                        float lastAvailableSpawnPos_z = relocateable.position.z;
                        float stepZ = (lastHitPosition.z - relocateable.position.z) / iterations;

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
        

        //Listen to OnDrag from Raycaster
        public void RelocateTarget(RaycastHit hitInfo)
        {
            if (isSelectedRelocateble)
            {
                needRelocateTarget = true;
                lastHitPosition = hitInfo.point;
            }   
        }

        //Listen to ClickedUp from Input
        public void UnselectTarget()
        {
            if (isSelectedRelocateble)
            {
                needRelocateTarget = false;
                isSelectedRelocateble = false;
                relocateable.gameObject.layer = selectedObjectLayer;
                relocateable = null;
                relocateableScript = null;
            }
        }

        private void SetPosition(Vector3 pos)
        {
            if (relocateableScript.TryPlace(pos)) relocateable.position = pos;
        }

        private void SetPosition(Vector3 pos, out bool isPosSet)
        {
            isPosSet = relocateableScript.TryPlace(pos);
            if (isPosSet) relocateable.position = pos;
        }
    }
}
