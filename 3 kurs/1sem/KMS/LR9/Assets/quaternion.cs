using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quaternion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion rot = Quaternion.AngleAxis(5, new Vector3(1, 2, 3));
		transform.rotation *= rot;
	}
}
