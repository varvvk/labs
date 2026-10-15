using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizontal_mouseX : MonoBehaviour {

	float speed = 0.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float my = Input.GetAxis("Mouse X");
		float mx = Input.GetAxis("Mouse Y");
		transform.Rotate(mx, my, 0);

		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		transform.Translate(x*speed, y*speed, 0);
	}
}
