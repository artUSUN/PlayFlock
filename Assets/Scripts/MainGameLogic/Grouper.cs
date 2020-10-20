using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayFlock.MainGameLogic
{
    public class Grouper : MonoBehaviour
    {
        [SerializeField] private Transform containterForGroups;

        private string groupName = "New Group";
        private bool isGrouping = false;
        private Transform firstElement, secondElement;
        private IGroupable firstScript, secondScript;


        //Listen to OnOneClicked from Raycaster
        public void CheckState(RaycastHit hitInfo)
        {
            firstScript = hitInfo.transform.GetComponent<IGroupable>();

            if (firstScript != null && isGrouping == false) StartGrouping(hitInfo);
            else StopGrouping(hitInfo);
        }
         
        private void StartGrouping(RaycastHit hitInfo)
        {
            isGrouping = true;
            firstElement = hitInfo.transform;
            //start grouping event (send 1st Transform)
        }

        private void StopGrouping(RaycastHit hitInfo)
        {
            secondElement = hitInfo.transform;

            if (firstScript == null) GroupingFailed();
            else if (secondElement == firstElement) GroupingFailed();
            else
            {
                IGroupable first = firstElement.GetComponent<IGroupable>();
                IGroupable second = secondElement.GetComponent<IGroupable>();

                if (!first.IsInGroup() && !second.IsInGroup())
                {
                    CreateNewGroup(first, second);
                    return;
                }
                if (first.IsInGroup() && !second.IsInGroup())
                {
                    AddElementInGroup(first.GetGroup(), second); //делигировать груп скрипту
                    return;
                }
                if (!first.IsInGroup() && second.IsInGroup())
                {
                    AddElementInGroup(second.GetGroup(), first); //делигировать груп скрипту
                    return;
                }
                if (first.IsInGroup() && second.IsInGroup())
                {
                    if (first.GetGroup() == second.GetGroup()) MergeGroups(first.GetGroup(), second.GetGroup());
                    else GroupingFailed();
                }
            }
        }

        private void AddElementInGroup(Group group, IGroupable element)
        {
            throw new NotImplementedException();
        }

        private void CreateNewGroup(IGroupable first, IGroupable second)
        {
            //event GroupCreated(send second element tansform)
            var group = new GameObject(groupName);
            group.transform.SetParent(containterForGroups);
            var groupScript = group.AddComponent<Group>();
            groupScript.Add(first, second);
            firstElement.SetParent(group.transform);
            secondElement.SetParent(group.transform);
        }

        private void MergeGroups(Group group1, Group group2)
        {
            throw new NotImplementedException();
        }

        private void GroupingFailed()
        {
            isGrouping = false;
            firstElement = null;
            secondElement = null;
            //grouping failed event void
        }

        private bool IsHitTagetIsGroupable(RaycastHit hit)
        {
            IGroupable fir = hit.transform.GetComponent<IGroupable>();
            return fir != null;
        }
    }
}
