using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Relocator : MonoBehaviour
    {
        [SerializeField] private float closeStep = 0.02f;

        private const int ignoreRaycastLayer = 2; 
        private int selectedObjectLayer;
        private Transform target;
        private IRelocateable relocateableScript;
        private bool isSelectedRelocateble = false, needRelocateTarget = false;
        private Vector3 lastHitPosition;


        //Listen to OnClickedDown from Raycaster
        public void SelectTarget(RaycastHit hitInfo)
        {
            relocateableScript = hitInfo.transform.GetComponent<IRelocateable>();

            if (relocateableScript != null)
            {
                target = hitInfo.transform;
                isSelectedRelocateble = true;
                selectedObjectLayer = target.gameObject.layer;
                target.gameObject.layer = ignoreRaycastLayer;
            }
        }

        private void Update()
        {
            if (needRelocateTarget)
            {
                if (relocateableScript.TryPlace(lastHitPosition)) target.position = lastHitPosition;
                else
                {
                    //try teleport to 
                    var newVector3_TryChangeX = new Vector3(lastHitPosition.x, target.position.y, target.position.z);
                    var newVector3_TryChangeZ = new Vector3(target.position.x, target.position.y, lastHitPosition.z);

                    SetPosition(newVector3_TryChangeX, out bool successX);
                    SetPosition(newVector3_TryChangeZ, out bool successY);

                    if ((successX | successY) == false)
                    {
                        float new_XPos = Mathf.MoveTowards(target.position.x, lastHitPosition.x, closeStep * Time.deltaTime);
                        float new_ZPos = Mathf.MoveTowards(target.position.z, lastHitPosition.z, closeStep * Time.deltaTime);

                        newVector3_TryChangeX = new Vector3(new_XPos, target.position.y, target.position.z);
                        newVector3_TryChangeZ = new Vector3(target.position.x, target.position.y, new_ZPos);

                        if (relocateableScript.TryPlace(newVector3_TryChangeX)) target.position = newVector3_TryChangeX;
                        if (relocateableScript.TryPlace(newVector3_TryChangeZ)) target.position = newVector3_TryChangeZ;
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
                target.gameObject.layer = selectedObjectLayer;
                target = null;
                relocateableScript = null;
            }
        }

        private void SetPosition(Vector3 pos)
        {
            if (relocateableScript.TryPlace(pos)) target.position = pos;
        }

        private void SetPosition(Vector3 pos, out bool isPosSet)
        {
            isPosSet = relocateableScript.TryPlace(pos);
            if (isPosSet) target.position = pos;
        }
    }
}
