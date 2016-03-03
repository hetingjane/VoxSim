﻿using UnityEngine;
using System.Collections;

public class BoneRotation : MonoBehaviour 
{
	Animator anim;
	private float x;
	private float y;
	private float z;
	private float speed = 5.0f;
	private float minLimit = -180.00f;
	private float maxLimit = 180.00f;
	Quaternion rotation_bone;
	public HumanBodyBones testBone;

	void Start() {
		anim = GetComponent<Animator> ();
		x = anim.GetBoneTransform (testBone).localRotation.x;
		y = anim.GetBoneTransform (testBone).localRotation.y;
		z = anim.GetBoneTransform (testBone).localRotation.z;
	}

	void LateUpdate() {
		// These optional code are for using the mouse to determine rotation, rather than keys. I found that it was difficult to use a 2-D pointer
		// to indicate 3-D space, so keys were implemented instead.

//		x += Input.GetAxis("Mouse X") * speed;
//		x = ClampAngle(x, minLimit, maxLimit);
//		y -= Input.GetAxis ("Mouse Y") * speed;
//		y = ClampAngle (y, minLimit, maxLimit);
//		rotation_bone = Quaternion.Euler(0, 0, x);
//		anim.GetBoneTransform(testBone).transform.localRotation = rotation_bone;
		if (Input.GetKey (KeyCode.A)) {
			x -= speed;
		} 
		if (Input.GetKey (KeyCode.D)) {
			x += speed;
		}
		if (Input.GetKey (KeyCode.E)) {
			y -= speed;
		} 
		if (Input.GetKey (KeyCode.Q)) {
			y += speed;
		}
		if (Input.GetKey (KeyCode.S)) { 
			z += speed;
		}
		if (Input.GetKey (KeyCode.W)) {
			z -= speed;
		}
		x = ClampAngle (x, minLimit, maxLimit);
		y = ClampAngle (y, minLimit, maxLimit);
		z = ClampAngle (z, minLimit, maxLimit);
		rotation_bone = Quaternion.Euler(z, y, x);
		anim.GetBoneTransform(testBone).transform.localRotation = rotation_bone;
	}
	
	static float ClampAngle (float angle, float min, float max) {
		if (angle < -360){
			angle += 360;
		}
		if (angle > 360){
			angle -= 360;
		}
		return Mathf.Clamp (angle, min, max);
	} 
}