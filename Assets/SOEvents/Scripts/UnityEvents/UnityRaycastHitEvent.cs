using UnityEngine;
using UnityEngine.Events;

namespace SOEvents
{
    [System.Serializable] public class UnityRaycastHitEvent : UnityEvent<RaycastHit> { }
}