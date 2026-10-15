using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_GetAxis : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		transform.Translate(x, 0, z);

		float x2 = Input.GetAxis("Mouse Y");
		float y = Input.GetAxis("Mouse X");

		x2 = Mathf.Clamp(x2, 0, 90);

		transform.Rotate(x2, -y, 0);
	}
}
