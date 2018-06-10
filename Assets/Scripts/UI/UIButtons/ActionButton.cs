using UnityEngine;
using System.Collections;

using Global;
using Network;
using VideoCapture;

public class ActionButton : UIButton {
    RestClient eventRestClient;
    protected EventManager eventManager;

    public int fontSize = 12;
    public string actionType;

	protected GUIStyle buttonStyle;
    protected TeachingCapture tc;

	// Use this for initialization
	protected void Start () {
		//buttonStyle = new GUIStyle ("Button");
		FontSizeModifier = (int)(fontSize / defaultFontSize);
        //buttonStyle.fontSize = fontSize;
        dimensions = new Vector2(100, 20);

        base.Start ();

        if (eventRestClient == null)
        {
            eventRestClient = GameObject.Find("CommunicationsBridge").GetComponent<RestClient>();
        }

        eventManager = GameObject.Find("BehaviorController").GetComponent<EventManager>();

        tc = GameObject.FindWithTag("MainCamera").GetComponent<TeachingCapture>();
        Debug.Log(GameObject.FindWithTag("MainCamera"));
        Debug.Log(tc);
    }

	// Update is called once per frame
	void Update () {

	}	

	protected virtual void OnGUI () {
		buttonStyle = new GUIStyle ("Button");
		buttonStyle.fontSize = fontSize;

		if (GUI.Button (buttonRect, buttonText, buttonStyle)) {
           Capture();
        }

		base.OnGUI ();
	}

    protected void Capture()
    {
        tc.captureDemonstrations();
        //eventManager.InsertEvent("", 0);

        //// hard code here, but should be able to fix quickly
        //eventManager.InsertEvent("slide(block2,<-0.1; 1; 0.3>)", 1);
        //eventManager.InsertEvent("slide(block2,<-0.7; 1; 0.4>)", 2);
        //eventManager.InsertEvent("slide(block2,<-0.6; 1; 1>)", 3);
        //eventManager.InsertEvent("slide(block2,<0.2; 1; 0.4>)", 4);

        ////eventManager.InsertEvent("upgrasp(block2)", 5);
    }
}
