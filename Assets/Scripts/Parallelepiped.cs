using PlayFlock;
using UnityEngine;

namespace PlayFlock
{
    public class Parallelepiped : MonoBehaviour, ISpawnable
    {
        [SerializeField] private LayerMask whatIsNotPlane;

        private Renderer rend;

        private void Start()
        {
            rend = GetComponent<Renderer>();
            rend.material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f));
        }

        public bool TrySpawn(Vector3 coordinates)
        {
            return !Physics.CheckBox(coordinates, new Vector3(transform.localScale.x / 2,
                                                             transform.localScale.y / 2,
                                                             transform.localScale.z / 2), Quaternion.identity, whatIsNotPlane);
        }
    }
}
