using PlayFlock.MainGameLogic;
using UnityEngine;

namespace PlayFlock
{
    public class Parallelepiped : InteractiveObject
    {
        [SerializeField] private LayerMask whatIsInteractive;
        private Renderer rend;

        protected override void Start()
        {
            base.Start();
            rend = GetComponent<Renderer>();
            rend.material.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.2f, 1f, 1f, 1f));
        }

        public override bool TryPlace(Vector3 coordinates)
        {
            return !Physics.CheckBox(coordinates, new Vector3(transform.localScale.x / 2,
                                                              transform.localScale.y / 2,
                                                              transform.localScale.z / 2), Quaternion.identity, whatIsInteractive);
        }
    }
}
