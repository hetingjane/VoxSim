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

namespace VideoCapture {
	public class TeachingCapture : MonoBehaviour {
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

		List<GameObject> eventObjs;

		Dictionary<string,string> paramValues;

		// Use this for initialization
		void Start () {
			recorder = gameObject.GetComponent<FlashbackRecorder> ();
			inputController = GameObject.Find ("IOController").GetComponent<InputController> ();
			eventManager = GameObject.Find ("BehaviorController").GetComponent<EventManager> ();
			objSelector = GameObject.Find ("BlocksWorld").GetComponent<ObjectSelector> ();
//			commBridge = GameObject.Find ("CommunicationsBridge").GetComponent<PluginImport> ();
//			preds = GameObject.Find ("BehaviorController").GetComponent<Predicates> ();

			FileWritten += CaptureComplete;
		}
		
		// Update is called once per frame
		void Update () {
			if (stopCaptureFlag) {
				SaveCapture ();
				stopCaptureFlag = false;
			}

			if ((writingFile) && (recorder.GetNumberOfPendingFiles () == 0)) {
				Debug.Log ("File written to disk.");
				OnFileWritten (this, null);
				writingFile = false;
			}

			if ((!capturing) && (!writingFile)) {
				if (Input.GetKeyDown (startCaptureKey)) {
					StartCapture (null, null);
				}
			}

			if (!writingFile) {
				if (Input.GetKeyDown (stopCaptureKey)) {
					StopCapture(null, null);
				}
			}
		}

		void StartCapture(object sender, EventArgs e) {
			if (!capturing) {

				recorder.StartCapture ();
				Debug.Log ("Starting video capture...");

				capturing = true;
				stopCaptureFlag = false;
			}
		}

		void SaveCapture () {
			recorder.StopCapture ();

			recorder.SaveCapturedFrames ();
			//recorder.SaveCapturedFrames (outFileName); // if custom filename


			capturing = false;
			writingFile = true;

			Debug.Log ("Stopping video capture.");
		}

		void StopCapture(object sender, EventArgs e) {
			if (capturing) {
				stopCaptureFlag = true;
			}
		}
			

		void CaptureComplete(object sender, EventArgs e) {
			// queue next one
		}
	}
}