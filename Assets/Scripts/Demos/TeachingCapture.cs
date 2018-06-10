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
        DemonstrationObject obs;

        // np.random.permutation(150)
        int[] hardcodePermutation = new int[] { 121, 82, 91, 46, 21, 2, 94, 13, 78, 74, 64, 93, 76,
                                                49, 66, 113, 126, 132, 3, 100, 68, 20, 115, 99, 124, 102,
                                                83, 34, 109, 10, 25, 58, 96, 90, 144, 26, 32, 22, 24,
                                               131, 88, 97, 38, 31, 87, 134, 54, 17, 122, 40, 50, 61,
                                                42, 36, 123, 51, 56, 44, 4, 5, 15, 125, 106, 95, 143,
                                               147, 79, 139, 130, 60, 112, 116, 133, 35, 137, 8, 80, 84,
                                               105, 118, 62, 39, 85, 63, 117, 72, 14, 92, 71, 43, 140,
                                                67, 12, 69, 11, 110, 108, 145, 57, 114, 138, 127, 128, 65,
                                                52, 141, 81, 86, 53, 136, 75, 55, 47, 129, 120, 41, 0,
                                                73, 9, 104, 89, 28, 1, 16, 148, 29, 77, 149, 18, 111,
                                                59, 37, 101, 33, 119, 19, 23, 45, 107, 6, 30, 70, 142,
                                                 7, 27, 135, 146, 98, 103, 48};

        /**
         * Get a demonstration from the full list of 150 demonstrations
         * 
         * index is its true index in the orignal list all_demos.json
         * 
         * Orders of actions in this list is Away -> Closer -> Past -> Around -> Next
         */
        private Demonstration.Demonstration getDemoFromTrueIndex( int index )
        {
            int type = index / 30;
            int typeIndex = index % 30;

            switch (type)
            {
                case 0:
                    return obs.Away[typeIndex];
                case 1:
                    return obs.Closer[typeIndex];
                case 2:
                    return obs.Past[typeIndex];
                case 3:
                    return obs.Around[typeIndex];
                case 4:
                    return obs.Next[typeIndex];
                default:
                    return null;
            }
        }

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

            obs = JsonConvert.DeserializeObject<DemonstrationObject>(reader.ReadToEnd());

            FileWritten += CaptureComplete;
        }

        int loopTo = 0;
        int loopIndex = 0;

        public void captureDemonstrations()
        {
            //for (int i = 0; i < 10; i ++ )
            //{
            //    StartCoroutine(runDemonstration(i));
            //}

            StartCoroutine(runMultipleDemonstrations(0, 150));

            //runDemonstrationStraight(obs.Around[0]);
        }

        void runDemonstrationStraight(Demonstration.Demonstration d)
        {
            prepareDemonstration(d);

            demonstrate(d);
        }

        IEnumerator runMultipleDemonstrations( int from, int to )
        {
            loopIndex = from;
            loopTo = to;

            yield return runDemonstration();
            //for (int i = from; i < to; i++)
            //{
            //    yield return runDemonstration(i);
            //}
        }

        IEnumerator runDemonstration()
        {
            int trueIndex = hardcodePermutation[loopIndex];

            Demonstration.Demonstration d = getDemoFromTrueIndex(trueIndex);

            Debug.Log("========= DEMO ===========");

            //prepapre Demonstration
            prepareDemonstration(d);

            yield return new WaitForSeconds(1);

            StartCapture(null, null);

            yield return new WaitForSeconds(1);

            ////Wait for 4 seconds
            //yield return new WaitForSeconds(1);

            demonstrate(d);

            yield return new WaitForSeconds(3);

            SaveCapture(loopIndex + ".mp4");
        }


        private void prepareDemonstration(Demonstration.Demonstration d)
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

            var block1 = GameObject.Find("block1");
            Debug.Log(d.getFirstStart());
            block1.transform.position = new Vector3(d.getFirstStart().x, 1.04f, d.getFirstStart().y);
            (block1.GetComponent<Voxeme>() as Voxeme).targetPosition = block1.transform.position;

            var block2 = GameObject.Find("block2");
            Debug.Log(d.getSecondStart());
            block2.transform.position = new Vector3(d.getSecondStart().x, 1.04f, d.getSecondStart().y);
            (block2.GetComponent<Voxeme>() as Voxeme).targetPosition = block2.transform.position;
        }

        private void demonstrate(Demonstration.Demonstration d)
        {
            eventManager.InsertEvent("", 0);

            for (int i = 0; i < d.getNoOfActions(); i++)
            {
                Vector2 action = d.getAction(i);
                eventManager.InsertEvent("slide(block1,<" + action.x + "; 1.04; " + action.y + ">)", i + 1);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (stopCaptureFlag)
            {
                SaveCapture(null);
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

        void SaveCapture(string outFileName)
        {
            recorder.StopCapture();

            if (outFileName == null)
            {
                recorder.SaveCapturedFrames();
            } else
            {
                recorder.SaveCapturedFrames(outFileName); // if custom filename
            }
            


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
            Debug.Log("==Capture Completed==");
            // queue next one
            loopIndex++;
            if (loopIndex < loopTo)
            {
                StartCoroutine(runDemonstration());
            }
        }
    }
}