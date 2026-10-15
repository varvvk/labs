using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Quaternion : MonoBehaviour {

	Quaternion orig;

	public float angle;

	// Use this for initialization
	void Start () {
		orig = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		angle += 1f;
		Quaternion rotX = Quaternion.AngleAxis(angle, Vector3.right);
		Quaternion rotZ = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = orig * rotX * rotZ;
	}
}
