using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PlayFlock.GameLogic
{
    public class Relocator : MonoBehaviour
    {
        [SerializeField] private float closeDistanse = 5f;
        [SerializeField] private float farStep = 0.5f;
        [SerializeField] private float closeStep = 0.02f;
        [SerializeField] private LayerMask whatIsInteractive;

        private const int interactiveLayer = 9, selectedObjectLayer = 10;
        private Transform target;
        private IRelocateable relocateableScript;
        private bool isSelectedRelocateble = false, needRelocateTarget = false;
        private Vector3 lastHitPosition;

        private void Start()
        {
            whatIsInteractive = LayerMask.GetMask("Interactive");
        }

        //Listen to OnClickedDown from Raycaster
        public void SelectTarget(RaycastHit hitInfo)
        {
            if (1 << hitInfo.collider.gameObject.layer == whatIsInteractive.value)
            {
                isSelectedRelocateble = true;
                target = hitInfo.collider.transform;
                target.gameObject.layer = selectedObjectLayer;
                relocateableScript = target.GetComponent<IRelocateable>();
            }
        }

        private void Update()
        {
            if (needRelocateTarget)
            {
                //try to relocate to mouse pos
                if (relocateableScript.TryPlace(lastHitPosition)) target.position = lastHitPosition;
                //if cant
                else
                {
                    Vector3 delta = target.position - lastHitPosition;
                    Vector3 newPos;
                    //check distance between center of object and mouse pos, and if it large - move obj fast
                    if (delta.sqrMagnitude > closeDistanse)
                    {
                        newPos = Vector3.MoveTowards(target.position, lastHitPosition, farStep);
                        if (relocateableScript.TryPlace(newPos)) target.position = newPos;
                    }
                    //then try move slow
                    newPos = Vector3.MoveTowards(target.position, lastHitPosition, closeStep);
                    if (relocateableScript.TryPlace(newPos)) target.position = newPos;
                    //if cant, try to move only on x or y
                    else
                    {
                        float new_XPos = Mathf.MoveTowards(target.position.x, lastHitPosition.x, closeStep);
                        float new_YPos = Mathf.MoveTowards(target.position.y, lastHitPosition.y, closeStep);

                        Vector3 newVector3_TryChangeX = new Vector3(new_XPos, target.position.y, target.position.z);
                        Vector3 newVector3_TryChangeY = new Vector3(target.position.x, new_YPos, target.position.z);

                        if (relocateableScript.TryPlace(newVector3_TryChangeX)) target.position = newVector3_TryChangeX;
                        else if (relocateableScript.TryPlace(newVector3_TryChangeY)) target.position = newVector3_TryChangeY;
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
                target.gameObject.layer = interactiveLayer;
                target = null;
                relocateableScript = null;
            }
        }
    }
}
