using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Group : MonoBehaviour
    {
        private List<GroupElement> elements;

        private Group()
        {
            Start();
        }

        private void Start()
        {
            elements = new List<GroupElement>();
        }

        public void Add(Transform element, IGroupable elementScript)
        {
            elements.Add(new GroupElement(element, elementScript, element.parent));
            element.SetParent(transform);
            elementScript.AddInGroup(this);
        }

        public void Add(Transform element1, IGroupable elementScript1, Transform element2, IGroupable elementScript2)
        {
            Add(element1, elementScript1);
            Add(element2, elementScript2);
        }
    }
}
