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
            var groupableCheck = hitInfo.transform.GetComponent<IGroupable>();

            if (groupableCheck != null && isGrouping == false)
            {
                firstScript = groupableCheck;
                StartGrouping(hitInfo);
            }
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
            secondScript = secondElement.GetComponent<IGroupable>();

            //If at least one object is not groupable then failure
            if (firstScript == null || secondScript == null) GroupingEnded();
            //Else if it is the same object then failure
            else if (secondElement == firstElement) GroupingEnded();
            //Else try to group
            else TryToGroup(firstScript, secondScript);
        }

        private void TryToGroup(IGroupable first, IGroupable second)
        {
            if (!first.IsInGroup() && !second.IsInGroup())
            {
                CreateNewGroup(first, second);
                return;
            }
            if (first.IsInGroup() && !second.IsInGroup())
            {
                AddElementInGroup(firstElement, secondElement); //делигировать груп скрипту
                return;
            }
            if (!first.IsInGroup() && second.IsInGroup())
            {
                AddElementInGroup(secondElement, firstElement); //делигировать груп скрипту
                return;
            }
            if (first.IsInGroup() && second.IsInGroup())
            {
                if (first.GetGroup() != second.GetGroup()) MergeGroups(first.GetGroup(), second.GetGroup());
                else
                { 
                    //check chain
                }
            }
        }

        private void AddElementInGroup(Transform elementInGroup, Transform elementWithoutGroup)
        {
            //add in group
        }

        private void CreateNewGroup(IGroupable first, IGroupable second)
        {
            //event GroupCreated(send second element tansform) - ненад
            var group = new GameObject(groupName);
            group.transform.SetParent(containterForGroups);
            var groupScript = group.AddComponent<Group>();
            groupScript.Add(firstElement, first, secondElement, second); //заменить адд на create
            GroupingEnded();
        }

        private void MergeGroups(Group group1, Group group2)
        {
            throw new NotImplementedException();
        }

        private void GroupingEnded()
        {
            isGrouping = false;
            firstElement = null;
            secondElement = null;
            firstScript = null;
            secondScript = null;
            //grouping ended event void
        }
    }
}
