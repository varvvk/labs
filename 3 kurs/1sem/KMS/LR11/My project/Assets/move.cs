using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	public float speed = 0.08f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float dX = Input.GetAxis("Horizontal");
        float dZ = Input.GetAxis("Vertical");
        transform.Translate(dX * speed, 0, dZ * speed);

        float dYm = Input.GetAxis("Mouse X");
        transform.Rotate(0, dYm, 0);
    }
}
