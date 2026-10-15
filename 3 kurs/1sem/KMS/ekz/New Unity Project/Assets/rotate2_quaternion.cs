using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate2_quaternion : MonoBehaviour {

	Quaternion orig;
	public float angle;
	// Use this for initialization
	void Start () {
		orig = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		angle += 1;
		Quaternion rot1 = Quaternion.AngleAxis(angle, Vector3.up);
		transform.rotation = rot1 * orig;
	}
}
