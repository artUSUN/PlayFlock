using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class LinePainter : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(0, 0.51f, 0);
        [SerializeField] private Transform containerForLines;
        [SerializeField] private GameObject linePrefab;

        private GameObject tempLine;
        private string newBindLineName = "Bind Line", newTempLineName = "Temp Line";

        public void OnBindingStarted(Transform firstBindObject)
        {
            CreateTempLine(firstBindObject.position);
        }

        public void OnBindingSuccess(Transform[] transforms)
        {
            CreateBindLine(transforms);
        }

        public void OnBindingFinished()
        {
            if (tempLine != null) DestroyTempLine();
        }  

        private void CreateTempLine(Vector3 firstPos)
        {
            if (tempLine != null) DestroyTempLine();
            tempLine = Instantiate(linePrefab);
            tempLine.name = newTempLineName;
            tempLine.transform.SetParent(transform);
            tempLine.AddComponent<BindLine_FollowMouse>();
            var tempLineScript = tempLine.AddComponent<BindLine_FollowMouse>();
            tempLineScript.Init(firstPos + offset);
        }

        private void DestroyTempLine()
        {
            Destroy(tempLine);
        }

        private void CreateBindLine(Transform[] transforms)
        { 
            var newLine = Instantiate(linePrefab);
            newLine.name = newBindLineName;
            newLine.transform.SetParent(containerForLines);
            var bindLineScript = newLine.AddComponent<BindLine>();
            bindLineScript.Init(transforms[0], transforms[1], offset);
        }
    }
}
