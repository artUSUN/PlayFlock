using SOEvents;
using System;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] private float rayLength = 100f;
        [SerializeField] private LayerMask whatIsPlane;
        [SerializeField] private RaycasterEvents events;

        private Camera cam;
        private Vector2 lastCachedMousePos;
        private RaycastHit lastRaycastHit;


        private void Start()
        {
            cam = Camera.main;
        }

        //Listen to OnClickedDown from Input
        public void OnClickedDown(Vector2 clickPos)
        {
            lastCachedMousePos = clickPos;
            lastRaycastHit = Cast(clickPos);
            events.ClickDown.Raise(lastRaycastHit);
        }

        //Listen to OnOneClicked from Input
        public void OnOneClicked(Vector2 clickPos)
        {
            if (clickPos == lastCachedMousePos)
            {
                events.OneClick.Raise(lastRaycastHit);
            }
        }

        //Listen to OnDoubleClicked from Input
        public void OnDoubleClicked(Vector2 clickPos)
        {
            events.DoubleClick.Raise(Cast(clickPos));
        }

        //Listen to OnDrag from Input
        public void OnDrag(Vector2 currentPos)
        {
            lastRaycastHit = Cast(currentPos, whatIsPlane);
            events.Drag.Raise(lastRaycastHit);
        }

        private RaycastHit Cast(Vector2 screenPos)
        {
            Ray ray = cam.ScreenPointToRay(screenPos);
            Physics.Raycast(ray, out RaycastHit hitInfo, rayLength);
            return hitInfo;
        }

        private RaycastHit Cast(Vector2 screenPos, LayerMask layerMask)
        {
            Ray ray = cam.ScreenPointToRay(screenPos);
            Physics.Raycast(ray, out RaycastHit hitInfo, rayLength, layerMask);
            return hitInfo;
        }

        [Serializable]
        public struct RaycasterEvents
        {
            public RaycastHitEvent ClickDown;
            public RaycastHitEvent OneClick;
            public RaycastHitEvent DoubleClick;
            public RaycastHitEvent Drag;
        }
    }
}
