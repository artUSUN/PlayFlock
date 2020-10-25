using SOEvents;
using System;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Binder : MonoBehaviour
    {
        [SerializeField] private BinderEvents events;

        private IBindable firstElementScript, secondElementScript;
        private BinderState currentState, pastState;
        private Transform firstElementTransform;

        private void Awake()
        {
            
        }

        //Listen to OnOneClick from Raycaster
        public void OnOneClick(RaycastHit hitInfo)
        {
            switch (currentState)
            {
                case BinderState.Waiting:
                    {
                        firstElementScript = hitInfo.transform.GetComponent<IBindable>();

                        if (firstElementScript != null)
                        {
                            currentState = BinderState.Binding;
                            firstElementTransform = hitInfo.transform;
                            events.BindingStarted.Raise(firstElementTransform);
                        }
                    }
                    break;

                case BinderState.Binding:
                    {
                        secondElementScript = hitInfo.transform.GetComponent<IBindable>();

                        if (secondElementScript == null) FinishBinding();
                        else if (firstElementScript == secondElementScript) FinishBinding();
                        else if (firstElementScript.IsInBindWith(secondElementScript)) RemoveBind();
                        else AddBind();
                    }
                    break;
            }
        }

        //Listen to OnDoubleClick from Raycaster
        public void OnDoubleClick()
        {
            FinishBinding();
        }

        private void RemoveBind()
        {
            firstElementScript.RemoveBind(secondElementScript);
            secondElementScript.RemoveBind(firstElementScript);
            FinishBinding();
        }

        private void AddBind()
        {
            firstElementScript.AddBind(secondElementScript, out var firstBindingPoint);
            secondElementScript.AddBind(firstElementScript, out var secondBindingPoint);
            events.BindingSuccess.Raise(new Transform[] { firstBindingPoint, secondBindingPoint });
            FinishBinding();
        }

        private void FinishBinding()
        {
            firstElementTransform = null;
            firstElementScript = null;
            secondElementScript = null;
            currentState = BinderState.Waiting;
            events.BindingFinished.Raise();
        }
    }

    public enum BinderState
    {
        Waiting,
        Binding
    }

    [Serializable]
    public struct BinderEvents
    {
        public TransformEvent BindingStarted;
        public VoidEvent BindingFinished;
        public TransformArrayEvent BindingSuccess;
    }
}
