using UnityEngine;
using System.Collections;

using Global;

public class ActionButton : UIButton {

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
	}

	// Update is called once per frame
	void Update () {

	}	

	protected virtual void OnGUI () {
		buttonStyle = new GUIStyle ("Button");
		buttonStyle.fontSize = fontSize;

		if (GUI.Button (buttonRect, buttonText, buttonStyle)) {
			return;
		}

		base.OnGUI ();
	}
}
