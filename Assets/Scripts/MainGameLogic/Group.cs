using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Group : MonoBehaviour
    {
        private List<IGroupable> elements;

        private Group()
        {
            Start();
        }

        private void Start()
        {
            elements = new List<IGroupable>();
        }

        public void Add(IGroupable element)
        {
            elements.Add(element);
            element.AddInGroup(this);
        }

        public void Add(IGroupable element1, IGroupable element2)
        {
            elements.Add(element1);
            elements.Add(element2);
        }
    }
}
