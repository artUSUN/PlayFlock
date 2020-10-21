using SOEvents;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace PlayFlock
{
    public class InputHandler : MonoBehaviour, IDragHandler, IPointerClickHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private InputEvents events;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 1)
            {
                events.OneClickPos.Raise(eventData.position);
            }

            if (eventData.clickCount >= 2)
            {
                events.DoubleClickPos.Raise(eventData.position);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            events.DragPos.Raise(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            events.EndDrag.Raise(); 
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            events.ClickDownPos.Raise(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            events.ClickUp.Raise();
        }
    }

    [Serializable]
    public struct InputEvents
    {
        public Vector2Event OneClickPos;
        public Vector2Event DoubleClickPos;
        public Vector2Event DragPos;
        public VoidEvent EndDrag;
        public Vector2Event ClickDownPos;
        public VoidEvent ClickUp;
    }

}
