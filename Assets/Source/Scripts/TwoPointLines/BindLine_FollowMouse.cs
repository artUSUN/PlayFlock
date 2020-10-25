using UnityEngine;

namespace PlayFlock.TwoPointLines
{
    public class BindLine_FollowMouse : TwoPointLine
    {
        private Camera cam;
        private float firstPos_Y;

        public void Init(Vector3 pos)
        {
            base.SetFirstPos(pos);
            firstPos_Y = pos.y;
            SetSecondPos();
        }

        private void SetSecondPos()
        {
            Vector2 mousePos = Input.mousePosition;
            var point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - firstPos_Y));
            base.SetSecondPos(point);
        }

        protected override void Awake()
        {
            base.Awake();
            cam = Camera.main;
        }

        private void Update()
        {
            SetSecondPos();
        }

        
    }
}
