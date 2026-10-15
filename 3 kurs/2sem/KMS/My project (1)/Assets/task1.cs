using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task1 : MonoBehaviour {

	public Transform targetPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(1))
		{
			float y = Input.GetAxis("Mouse X");
			transform.RotateAround(targetPos.position, Vector3.up, y);
		}
	}
}
