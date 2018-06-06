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
        RestClient eventRestClient;

        new void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (eventLearningClient == null)
            {
                eventLearningClient = GameObject.Find("CommunicationsBridge").GetComponent<PluginImport>().EventLearningClient;

				if (eventLearningClient != null) {
					eventLearningClient.EventSequenceReceived += ReceivedEvent;
				}
            }

            if (eventRestClient == null)
            {
                eventRestClient = GameObject.Find("CommunicationsBridge").GetComponent<RestClient>();
            }
        }

        private void ReceivedEvent(object sender, EventArgs e)
        {
            string commandMessage = ((EventLearningEventArgs)e).Content;
            Debug.Log(" ========= commandMessage ========== " );
            Debug.Log(commandMessage);

            eventManager.InsertEvent("", 0);
			eventManager.InsertEvent("slide(block1,<0; 0.8; 0>)", 1);
            eventManager.InsertEvent("slide(block1,<0.2; 0.8; 0.5>)", 2);

            // static ( -0.5, 0.6 )
            // moving point ( -0.1, 0.3 )
            // moving point ( -0.7, 0.4 )
            // moving point ( -0.6, 1 )
            // moving point ( 0.2, 0.4 )
        }
    }
}
