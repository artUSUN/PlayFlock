using UnityEngine;

namespace PlayFlock.TwoPointLines
{
    public class BindLine : TwoPointLine
    {
        private Transform firstPoint, secondPoint, cachedTransform;
        private Vector3 firstPosWithoutOffset, secondPosWithoutOffset, offset;
        private bool isInit = false;

        protected override void Awake()
        {
            base.Awake();
            cachedTransform = transform;
        }

        private void Update()
        {
            if (isInit)
            {
                if (firstPoint == null || secondPoint == null)
                {
                    Destroy(transform.gameObject);
                    return;
                }

                if (firstPosWithoutOffset != firstPoint.position) SetFirstPos();
                if (secondPosWithoutOffset != secondPoint.position) SetSecondPos();
            }
        }

        private void SetFirstPos()
        {
            base.SetFirstPos(firstPoint.position + offset);
            firstPosWithoutOffset = firstPoint.position;
        }

        private void SetSecondPos()
        {
            base.SetSecondPos(secondPoint.position + offset);
            secondPosWithoutOffset = secondPoint.position;
        }

        public void Init(Transform firstPoint, Transform secondPoint, Vector3 offset)
        {
            this.firstPoint = firstPoint;
            this.secondPoint = secondPoint;
            this.offset = offset;
            SetFirstPos(firstPoint.position + offset);
            SetSecondPos(secondPoint.position + offset);
            isInit = true;
        }
    }
}
