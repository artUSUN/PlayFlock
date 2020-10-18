using SOEvents;
using UnityEngine;
using UnityEngine.EventSystems;


namespace PlayFlock
{
    public class InputHandler : MonoBehaviour, IDragHandler, IPointerClickHandler, IEndDragHandler, IScrollHandler, IPointerDownHandler
    {
        [SerializeField] private float clickDelay = 0.3f;
        
        [Header("Events")]
        [SerializeField] private Vector2Event event_OnePushPos;
        [SerializeField] private Vector2Event event_DoublePushPos;
        [SerializeField] private Vector2Event event_DragPos;
        [SerializeField] private Vector2Event event_EndDragPos;
        [SerializeField] private Vector2Event event_ScrollDelta;

        

        public void OnPointerClick(PointerEventData eventData)
        {

            if (eventData.clickCount >= 2)
            {
                event_DoublePushPos.Raise(eventData.position);
                Debug.Log("double click " + eventData.position);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            event_DragPos.Raise(eventData.position);
            Debug.Log("Dragging " + eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            event_EndDragPos.Raise(eventData.position);
            Debug.Log("Drag ended in " + eventData.position);
        }

        public void OnScroll(PointerEventData eventData)
        {
            event_ScrollDelta.Raise(eventData.scrollDelta);
            Debug.Log("scrolling " + eventData.scrollDelta);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            event_OnePushPos.Raise(eventData.position);
            Debug.Log("one click " + eventData.position); 
        }
    }
}
