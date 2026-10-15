using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	float speed = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float x = Input.GetAxis("Horizontal"); //AD
        float y = Input.GetAxis("Vertical"); //WS
        transform.Translate(x * speed, 0, y * speed);

    }

}
