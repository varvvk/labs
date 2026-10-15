using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_2 : MonoBehaviour {

    public float speed = 0.08f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float dX = 0f;
        float dZ = 0f;

        // движение по X
        if (Input.GetKey(KeyCode.E)) dX = -1f;
        if (Input.GetKey(KeyCode.R)) dX = 1f;

        // движение по Z
        if (Input.GetKey(KeyCode.T)) dZ = 1f;
        if (Input.GetKey(KeyCode.Y)) dZ = -1f;

        transform.Translate(dX * speed, 0, dZ * speed);
    }
}
