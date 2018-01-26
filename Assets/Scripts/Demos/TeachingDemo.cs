using Network;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Demos
{
    class TeachingDemo : CommonDemo
    {
        EventLearningClient eventLearningClient;

        new void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (eventLearningClient == null)
            {
                eventLearningClient = GameObject.Find("CommunicationsBridge").GetComponent<PluginImport>().EventLearningClient;
                eventLearningClient.EventSequenceReceived += ReceivedEvent;
            }
        }

        private void ReceivedEvent(object sender, EventArgs e)
        {
            string commandMessage = ((EventLearningEventArgs)e).Content;
            Debug.Log(" ========= commandMessage ========== " );
            Debug.Log(commandMessage);

            eventManager.InsertEvent("", 0);
            eventManager.InsertEvent("slide(block1)", 1);
        }
    }
}
