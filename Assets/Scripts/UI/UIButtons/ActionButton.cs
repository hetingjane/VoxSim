using UnityEngine;
using System.Collections;

using Global;
using Network;

public class ActionButton : UIButton {
    RestClient eventRestClient;

    public int fontSize = 12;
    public string actionType;

	protected GUIStyle buttonStyle;

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
    }

	// Update is called once per frame
	void Update () {

	}	

	protected virtual void OnGUI () {
		buttonStyle = new GUIStyle ("Button");
		buttonStyle.fontSize = fontSize;

		if (GUI.Button (buttonRect, buttonText, buttonStyle)) {
           StartCoroutine(callLearningServer());
        }

		base.OnGUI ();
	}

    protected IEnumerator callLearningServer()
    {
        using (WWW www = new WWW("http://localhost:8000/learning/"))
        {
            yield return www;
            Debug.Log("Receive command ");
            Debug.Log(www.text);


        }
    }
}
