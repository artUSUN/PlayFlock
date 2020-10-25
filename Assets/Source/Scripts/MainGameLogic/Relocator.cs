using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Relocator : MonoBehaviour
    {
        [SerializeField] private float closeStep = 0.02f;
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
                    bool canSpawn;
                    Vector3 lastAvailableSpawnPos = relocateable.position;
                    Vector3 step = (lastHitPosition - relocateable.position)/iterations;
                    

                    //for (int i = 0; i < iterations; i++)
                    //{
                    //    lastAvailableSpawnPos = relocateable.position + step;
                    //    canSpawn = relocateableScript.TryPlace(lastAvailableSpawnPos);
                    //    if (canSpawn) 
                    //    {
                    //        relocateable.position = lastAvailableSpawnPos;
                    //        break;
                    //    }
                    //}

                    do
                    {
                        lastAvailableSpawnPos += step;
                        canSpawn = relocateableScript.TryPlace(lastAvailableSpawnPos);
                    } while (canSpawn);

                    relocateable.position = lastAvailableSpawnPos - step;

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

                //else
                //{
                //    //try teleport to 
                //    var newVector3_TryChangeX = new Vector3(lastHitPosition.x, target.position.y, target.position.z);
                //    var newVector3_TryChangeZ = new Vector3(target.position.x, target.position.y, lastHitPosition.z);

                //    SetPosition(newVector3_TryChangeX, out bool successX);
                //    SetPosition(newVector3_TryChangeZ, out bool successY);

                //    if ((successX & successY) == false)
                //    {
                //        float new_XPos = Mathf.MoveTowards(target.position.x, lastHitPosition.x, closeStep * Time.deltaTime);
                //        float new_ZPos = Mathf.MoveTowards(target.position.z, lastHitPosition.z, closeStep * Time.deltaTime);

                //        newVector3_TryChangeX = new Vector3(new_XPos, target.position.y, target.position.z);
                //        newVector3_TryChangeZ = new Vector3(target.position.x, target.position.y, new_ZPos);

                //        if (relocateableScript.TryPlace(newVector3_TryChangeX)) target.position = newVector3_TryChangeX;
                //        if (relocateableScript.TryPlace(newVector3_TryChangeZ)) target.position = newVector3_TryChangeZ;
                //    }
                //}
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
