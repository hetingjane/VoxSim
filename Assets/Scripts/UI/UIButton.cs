﻿using UnityEngine;
using System;
using System.Collections;

public enum UIButtonPosition {
	TopLeft,
	TopRight,
	BottomLeft,
	BottomRight
};

public class UIButton : FontManager
{
	public Rect buttonRect;
	public string buttonText;
	public int id;

	float fontSizeModifier;
	[HideInInspector]
	public float FontSizeModifier {
		get { return fontSizeModifier; }
		set { fontSizeModifier = value; }
	}

	public UIButtonPosition position;
	public Vector2 offset, dimensions;

	protected UIButtonManager buttonManager;

	// Use this for initialization
	protected virtual void Start () {
		buttonManager = GameObject.Find("BlocksWorld").GetComponent<UIButtonManager> ();

		id = buttonManager.buttonManager.Count;

		if (!buttonManager.buttonManager.ContainsKey (id)) {
			buttonManager.RegisterButton (this);
		}
		else {
			Debug.Log ("UIButton of id " + id.ToString () + " already exists on this object!");
			Destroy(this);
		}

		if (position == UIButtonPosition.TopLeft) {
			//int count = buttonManager.CountButtonsAtPosition (UIButtonPosition.TopLeft);
			buttonRect = new Rect (10 + offset.x, 10 + offset.y, dimensions.x, dimensions.y);
		} 
		else if (position == UIButtonPosition.TopRight) {
			//int count = buttonManager.CountButtonsAtPosition (UIButtonPosition.TopRight);
			buttonRect = new Rect (Screen.width - (10 + offset.x + dimensions.x), 10 + offset.y, dimensions.x, dimensions.y);
		}
		else if (position == UIButtonPosition.BottomLeft) {
			//int count = buttonManager.CountButtonsAtPosition (UIButtonPosition.BottomLeft);
			buttonRect = new Rect (10 + offset.x, Screen.height - (10 + offset.y + dimensions.y), dimensions.x, dimensions.y);
		}
		else if (position == UIButtonPosition.BottomRight) {
			//int count = buttonManager.CountButtonsAtPosition (UIButtonPosition.BottomRight);
			buttonRect = new Rect (Screen.width - (10 + offset.x + dimensions.x), Screen.height - (10 + offset.y +	 dimensions.y), dimensions.x, dimensions.y);
		}
	}

	// Update is called once per frame
	void Update () {
	}

	protected virtual void OnGUI() {
		//GUILayout automatically lays out the GUI window to contain all the text
		//GUI.Button(buttonRect, buttonText);
	}

	public virtual void DoUIButton(int buttonID){
		//Debug.Log (buttonID);
	}

	public virtual void DestroyButton() {
		buttonManager.UnregisterButton (this);
		Destroy (this);
	}
}
