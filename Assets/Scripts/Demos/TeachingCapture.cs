using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Timers;

using Global;
using FlashbackVideoRecorder;
using SQLite4Unity3d;
using Demonstration;
using System.Text;
using SimpleJSON;

using Newtonsoft.Json;

namespace VideoCapture
{
    public class TeachingCapture : MonoBehaviour
    {
        public KeyCode startCaptureKey;
        public KeyCode stopCaptureKey;

        FlashbackRecorder recorder;
        InputController inputController;
        EventManager eventManager;
        ObjectSelector objSelector;
        //		PluginImport commBridge;
        //		Predicates preds;

        public event EventHandler FileWritten;

        public void OnFileWritten(object sender, EventArgs e)
        {
            if (FileWritten != null)
            {
                FileWritten(this, e);
            }
        }

        bool capturing = false;
        bool writingFile = false;
        bool stopCaptureFlag = false;

        // Use this for initialization
        void Start()
        {
            recorder = gameObject.GetComponent<FlashbackRecorder>();
            inputController = GameObject.Find("IOController").GetComponent<InputController>();
            eventManager = GameObject.Find("BehaviorController").GetComponent<EventManager>();
            objSelector = GameObject.Find("BlocksWorld").GetComponent<ObjectSelector>();



            //string t = "{\"Away\": [{\"action_storage\": [[-0.7686572789821743, 0.13465892735628843],[-0.7324335699510277, -0.38898763904597955]],\"start_config\": [[0.4683071250788693, -0.5105128449125931],[0.4455294114921423, 0.3503960837973433]]}]}";

            //string t = "{\"action_storage\": [[-0.7686572789821743, 0.13465892735628843],[-0.7324335699510277, -0.38898763904597955]],\"start_config\": [[0.4683071250788693, -0.5105128449125931],[0.4455294114921423, 0.3503960837973433]]}";

            //Demonstration.Demonstration ob = JsonConvert.DeserializeObject<Demonstration.Demonstration>(t);

            String path = "Data\\demo\\all_demos.json";
            StreamReader reader = new StreamReader(path);

            DemonstrationObject obs = JsonConvert.DeserializeObject<DemonstrationObject>(reader.ReadToEnd());

            //for ( ob.Around )

            runDemonstration(obs.Around[0]);

            FileWritten += CaptureComplete;
        }

        void runDemonstration ( Demonstration.Demonstration d )
        {

            Debug.Log("========= DEMO ===========");

            var block1 = GameObject.Find("block1");
            Debug.Log(d.getFirstStart());
            block1.transform.position = new Vector3(d.getFirstStart().x, 1.04f, d.getFirstStart().y);
            var block2 = GameObject.Find("block2");
            Debug.Log(d.getSecondStart());
            block2.transform.position = new Vector3(d.getSecondStart().x, 1.04f, d.getSecondStart().y);

            eventManager.InsertEvent("", 0);
            // hard code here, but should be able to fix quickly
            eventManager.InsertEvent("slide(block2,<-0.1; 1; 0.3>)", 1);
            eventManager.InsertEvent("slide(block2,<-0.7; 1; 0.4>)", 2);
            eventManager.InsertEvent("slide(block2,<-0.6; 1; 1>)", 3);
            eventManager.InsertEvent("slide(block2,<0.2; 1; 0.4>)", 4);
        }

        void prepareDemonstrion ()
        {
            // Code from Module Object Creation
            // It looks like the code here takes charge of creating objects on the table
            // ModuleObjectCreation is in turn lies in
            // VoxemeCreationSandbox

            // Reset locations of two blocks
            //selectedObject.transform.position = new Vector3(selectRayhit.point.x,
            //                        preds.ON(new object[] { sandboxSurface }).y + surfacePlacementOffset, selectRayhit.point.z);

            // Possible set physic 
            //selectedObject.GetComponent<Rigging> ().ActivatePhysics (true);


        }

        // Update is called once per frame
        void Update()
        {
            if (stopCaptureFlag)
            {
                SaveCapture();
                stopCaptureFlag = false;
            }

            if ((writingFile) && (recorder.GetNumberOfPendingFiles() == 0))
            {
                Debug.Log("File written to disk.");
                OnFileWritten(this, null);
                writingFile = false;
            }

            if ((!capturing) && (!writingFile))
            {
                if (Input.GetKeyDown(startCaptureKey))
                {
                    StartCapture(null, null);
                }
            }

            if (!writingFile)
            {
                if (Input.GetKeyDown(stopCaptureKey))
                {
                    StopCapture(null, null);
                }
            }
        }

        void StartCapture(object sender, EventArgs e)
        {
            if (!capturing)
            {
                recorder.StartCapture();
                Debug.Log("Starting video capture...");

                capturing = true;
                stopCaptureFlag = false;
            }
        }

        void SaveCapture()
        {
            recorder.StopCapture();

            recorder.SaveCapturedFrames();
            //recorder.SaveCapturedFrames (outFileName); // if custom filename


            capturing = false;
            writingFile = true;

            Debug.Log("Stopping video capture.");
        }

        void StopCapture(object sender, EventArgs e)
        {
            if (capturing)
            {
                stopCaptureFlag = true;
            }
        }


        void CaptureComplete(object sender, EventArgs e)
        {
            // queue next one
        }
    }
}