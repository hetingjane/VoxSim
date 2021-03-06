﻿using UnityEngine;
using System.Collections;

using Global;

public class ResetUIButton : UIButton {

	public int fontSize = 12;

	protected GUIStyle buttonStyle;

	// Use this for initialization
	protected void Start () {
		//buttonStyle = new GUIStyle ("Button");
		FontSizeModifier = (int)(fontSize / defaultFontSize);
		//buttonStyle.fontSize = fontSize;

		base.Start ();
	}

	// Update is called once per frame
	void Update () {

	}	

	protected virtual void OnGUI () {
		buttonStyle = new GUIStyle ("Button");
		buttonStyle.fontSize = fontSize;

		if (GUI.Button (buttonRect, buttonText, buttonStyle)) {
			StartCoroutine(SceneHelper.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name));
			return;
		}

		base.OnGUI ();
	}
}
