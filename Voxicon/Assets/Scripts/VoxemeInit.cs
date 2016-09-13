﻿using UnityEngine;
using System.Collections;

using Global;

public class VoxemeInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ObjectSelector objSelector = GameObject.Find ("BlocksWorld").GetComponent<ObjectSelector> ();
		Macros macros = GameObject.Find ("BehaviorController").GetComponent<Macros> ();

		/* MAKE GLOBAL OBJECT RUNTIME ALTERATIONS */

		// get all objects
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		Voxeme voxeme;

		foreach (GameObject go in allObjects) {
			if (go.activeInHierarchy) {
				// set up all objects to enable consistent manipulation
				// (i.e.) flatten any pos/rot inconsistencies in modeling or prefab setup due to human error
				voxeme = go.GetComponent<Voxeme> ();
				if (voxeme != null) {	// object has Voxeme component
					GameObject container = new GameObject (go.name, typeof(Rigging), typeof(Voxeme));
					container.transform.position = go.transform.position;
					go.transform.parent = container.transform;
					go.name += "*";
					voxeme.enabled = false;
					//container.GetComponent<Entity> ().enabled = false;

					// copy attribute set
					AttributeSet newAttrSet = container.AddComponent<AttributeSet> ();
					AttributeSet attrSet = go.GetComponent<AttributeSet>();
					if (attrSet != null) {
						foreach (string s in attrSet.attributes) {
							newAttrSet.attributes.Add (s);
						}
					}
		
					// set up for physics
					// add box colliders and rigid bodies to all subobjects that have MeshFilters
					Renderer[] renderers = go.GetComponentsInChildren<Renderer> ();
					foreach (Renderer renderer in renderers) {
						GameObject subObj = renderer.gameObject;
						if (subObj.GetComponent<MeshFilter> () != null) {
							if (go.tag != "UnPhysic") {
								if (subObj.GetComponent<BoxCollider> () == null) {	// may already have one -- goddamn overachieving scene artists
									BoxCollider collider = subObj.AddComponent<BoxCollider> ();
									//Physics.IgnoreCollision (collider, GameObject.Find ("MainCamera").GetComponent<Collider> ());
								}
							}

							if ((go.tag != "UnPhysic") && (go.tag != "Ground")) {	// Non-physics objects are either scene markers or, like the ground, cognitively immobile
								if (subObj.GetComponent<Rigidbody> () == null) {	// may already have one -- goddamn overachieving scene artists
									Rigidbody rigidbody = subObj.AddComponent<Rigidbody> ();
									// assume mass is a volume of uniform density
									// assumption: all objects have the same density
									float x = Helper.GetObjectWorldSize (subObj).size.x;
									float y = Helper.GetObjectWorldSize (subObj).size.y;
									float z = Helper.GetObjectWorldSize (subObj).size.z;
									rigidbody.mass = x * y * z;

									// bunch of crap assumptions to calculate drag:
									// air density: 1.225 kg/m^3
									// flow velocity = parent voxeme moveSpeed
									// use box collider surface area for reference area
									// use Reynolds number for drag coefficient - assume 1
									// https://en.wikipedia.org/wiki/Drag_coefficient
									rigidbody.drag = 1.225f * voxeme.moveSpeed * ((2 * x * y) + (2 * y * z) + (2 * x * z)) * 1.0f;

									//rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
								}
							}
						}
					}
					// add to master voxeme list
					objSelector.allVoxemes.Add (container.GetComponent<Voxeme> ());
				}
			}
		}

		// set joint links between all subobjects (Cartesian product)
		foreach (GameObject go in allObjects) {
			if (go.activeInHierarchy) {
				Renderer[] renderers = go.GetComponentsInChildren<Renderer> ();
				foreach (Renderer r1 in renderers) {
					GameObject sub1 = r1.gameObject;
					foreach (Renderer r2 in renderers) {
						GameObject sub2 = r2.gameObject;
						if (sub1 != sub2) {
							FixedJoint fixedJoint = sub1.AddComponent<FixedJoint> ();
							fixedJoint.connectedBody = sub2.GetComponent<Rigidbody>();
						}
					}
				}
			}
		}

		macros.PopulateMacros ();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
