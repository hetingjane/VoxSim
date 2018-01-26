using Agent;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Demos
{
    class CommonDemo : MonoBehaviour
    {
        protected EventManager eventManager;
        InteractionSystem interactionSystem;

        GameObject Diana;

        IKControl ikControl;
        IKTarget leftTarget;
        IKTarget rightTarget;

        Vector3 leftTargetDefault, leftTargetStored;
        Vector3 rightTargetDefault, rightTargetStored;

        // Use this for initialization
        protected void Start()
        {
            eventManager = GameObject.Find("BehaviorController").GetComponent<EventManager>();
            //eventManager.EventComplete += ReturnToRest;

            Diana = GameObject.Find("Diana");
            ikControl = Diana.GetComponent<IKControl>();
            leftTargetDefault = ikControl.leftHandObj.transform.position;
            rightTargetDefault = ikControl.rightHandObj.transform.position;
        }

        void ReturnToRest(object sender, EventArgs e)
        {
            if (!interactionSystem.IsPaused(FullBodyBipedEffector.LeftHand) &&
                !interactionSystem.IsPaused(FullBodyBipedEffector.RightHand))
            {
                TurnForward();
            }
        }

        void TurnForward()
        {
            ikControl.leftHandObj.position = leftTargetDefault;
            ikControl.rightHandObj.position = rightTargetDefault;
            InteractionHelper.SetLeftHandTarget(Diana, ikControl.leftHandObj);
            InteractionHelper.SetRightHandTarget(Diana, ikControl.rightHandObj);
        }
    }
}
