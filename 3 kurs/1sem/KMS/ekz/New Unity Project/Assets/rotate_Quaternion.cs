using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_Quaternion : MonoBehaviour {

	public Quaternion orig_angle;
	public float angle;
	// Use this for initialization
	void Start () {

		orig_angle = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		angle += 1;
		Quaternion rot1 = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
		transform.rotation = orig_angle * rot1;
	}
}
