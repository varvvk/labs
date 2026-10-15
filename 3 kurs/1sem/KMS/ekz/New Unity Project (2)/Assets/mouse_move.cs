using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_move : MonoBehaviour {

	float speed = 0.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float x = Input.GetAxis("Mouse X");
		float y = Input.GetAxis("Mouse Y");

		transform.Translate(x*speed, y*speed, 0);
	}
}
